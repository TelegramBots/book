using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace LinkValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            string srcDirectory = Path.Combine(Environment.CurrentDirectory, "src/");
            int baseLength = srcDirectory.Length;

            List<string> files = GetAllFiles(srcDirectory, out List<string> allDirectories);

            HashSet<string> markdownEntities = new HashSet<string>();
            Dictionary<string, MarkdownFile> markdownFiles = new Dictionary<string, MarkdownFile>();
            foreach (var file in files)
            {
                string relativePath = file.Substring(baseLength).Replace('\\', '/');
                string relativeLowercasePath = relativePath.ToLower();
                markdownEntities.Add(relativeLowercasePath);

                if (file.EndsWith(".md"))
                    markdownFiles.Add(relativeLowercasePath, new MarkdownFile(file, relativePath, relativeLowercasePath));
            }
            foreach (var dir in allDirectories)
            {
                string dirLowercase = dir.Substring(baseLength).Replace('\\', '/').ToLower();
                if (markdownEntities.Contains(dirLowercase + "/readme.md"))
                    markdownEntities.Add(dirLowercase);
            }

            Console.WriteLine($"Indexed {files.Count} files, {markdownFiles.Count} of which are markdown files");

            foreach (var markdownFile in markdownFiles.Values)
                markdownFile.Parse();

            foreach (var markdownFile in markdownFiles.Values)
            {
                foreach (var (lineNr, reference) in markdownFile.References)
                {
                    if (reference.Contains('#') && !reference.Contains(':')) // Fragment
                    {
                        if (reference.OrdinalContains("##"))
                        {
                            ReportError("Fragment", reference, markdownFile.RelativePath, lineNr, "has double ##");
                            continue;
                        }

                        string fragment = reference.ToLower();
                        if (fragment.StartsWith('#')) // same-file local fragment identifier (e.g. #usage)
                        {
                            if (!markdownFile.HeaderEntities.Contains(fragment))
                                ReportError("Fragment identifier", reference, markdownFile.RelativePath, lineNr, "could not be resolved");
                        }
                        else // cross-file local fragment identifier (e.g. ./proxy.md#http-proxy
                        {
                            string markdownReference = fragment.SubstringBefore('#');
                            fragment = fragment.Substring(markdownReference.Length);

                            string referencedFilePath = CombineFilePaths(markdownFile.RelativeLowercasePath, markdownReference);

                            if (!markdownFiles.TryGetValue(referencedFilePath, out MarkdownFile referencedFile) ||
                                !referencedFile.HeaderEntities.Contains(fragment))
                                ReportError("Fragment identifier", reference, markdownFile.RelativePath, lineNr, "could not be resolved");
                        }
                    }
                    else if (reference.Contains(':')) // Link
                    {
                        TestLink(reference, markdownFile.RelativePath, lineNr);
                    }
                    else // File reference
                    {
                        string referencedFilePath = CombineFilePaths(markdownFile.RelativeLowercasePath, reference.ToLower());

                        if (!markdownEntities.Contains(referencedFilePath))
                            ReportError("Unresolved reference", reference, markdownFile.RelativePath, lineNr);
                    }
                }

                HashSet<string> definedReferenceNames = new HashSet<string>(
                    markdownFile.DefinedReferenceNames
                    .Select(reference => reference.referenceName.Trim().ToLower()));
                foreach (var (lineNr, name) in markdownFile.AccessedReferenceNames)
                {
                    if (!definedReferenceNames.Contains(name.Trim().ToLower()))
                        ReportError("Unresolved reference name", name, markdownFile.RelativePath, lineNr);
                }

                HashSet<string> accessedReferenceNames = new HashSet<string>(
                    markdownFile.AccessedReferenceNames
                    .Select(reference => reference.referenceName.Trim().ToLower()));
                foreach (var (lineNr, name) in markdownFile.DefinedReferenceNames)
                {
                    if (!accessedReferenceNames.Contains(name.Trim().ToLower()))
                        ReportWarning("Unused named reference", name, markdownFile.RelativePath, lineNr);
                }
            }

            if (ErrorsFound)
            {
                Environment.ExitCode = 1;
            }
            else if (!WarningsFound)
            {
                PrintColor("All references are good!\n", ConsoleColor.Green);
            }
        }

        #region Logging
        private static bool ErrorsFound = false;
        private static bool WarningsFound = false;

        private static void PrintColor(string message, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = previousColor;
        }
        private static string Format(string title, string reference, string file, int line, string message = null)
            => $"{title} `{reference}` in `{file}` on line {line}{(message == null ? "" : " " + message)}.";
        private static void ReportError(string title, string reference, string file, int line, string message = null)
        {
            PrintColor("Markdown error: ", ConsoleColor.Red);
            Console.WriteLine(Format(title, reference, file, line, message));
            ErrorsFound = true;
        }
        private static void ReportWarning(string title, string reference, string file, int line, string message = null)
        {
            PrintColor("Markdown warning: ", ConsoleColor.Red);
            Console.WriteLine(Format(title, reference, file, line, message));
            WarningsFound = true;
        }
        #endregion

        private static readonly Regex FragmentIdentifierRegex = new Regex(@"href=""#(.*?)""", RegexOptions.Compiled);
        private static readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler() { AllowAutoRedirect = false });
        private static readonly Dictionary<string, HashSet<string>> _visitedLinks = new Dictionary<string, HashSet<string>>();
        private static void TestLink(string link, string sourceFile, int line)
        {
            Queue<string> redirectQueue = new Queue<string>();

            const int MaxDepth = 5;

            void LinkWarning(string message, string replaceLink = null)
                => ReportWarning("Link", replaceLink ?? link, sourceFile, line, message);
            void LinkError(string message, string replaceLink = null)
                => ReportError("Link", replaceLink ?? link, sourceFile, line, message);

            bool TestLinkInternal(string requestUri)
            {
                void ReportMissingFragmentIdentifier(string fragmentIdentifier)
                    => LinkError($"contained a fragment identifier `#{fragmentIdentifier}`, that is missing on the target site", requestUri);

                try
                {
                    redirectQueue.Enqueue(requestUri);
                    if (redirectQueue.Count == MaxDepth)
                        return false;

                    string baseLink = requestUri;
                    string fragmentIdentifier = null;
                    if (requestUri.Contains('#'))
                    {
                        baseLink = baseLink.SubstringBefore('#');
                        fragmentIdentifier = requestUri.Substring(baseLink.Length + 1).Trim().ToLower();
                    }
                    baseLink.Trim();

                    if (_visitedLinks.TryGetValue(baseLink, out HashSet<string> identifiers))
                    {
                        if (fragmentIdentifier != null)
                        {
                            if (identifiers == null || !identifiers.Contains(fragmentIdentifier))
                            {
                                ReportMissingFragmentIdentifier(fragmentIdentifier);
                            }
                        }
                        return true;
                    }

                    using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
                    using (var response = _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result)
                    {
                        int statusCode = (int)response.StatusCode;
                        if (statusCode.IsInRange(200, 299))
                        {
                            HashSet<string> fragmentIdentifiers = new HashSet<string>();
                            if (response.Content.Headers.ContentType.MediaType == "text/html")
                            {
                                string htmlContent = response.Content.ReadAsStringAsync().Result;
                                foreach (Match match in FragmentIdentifierRegex.Matches(htmlContent))
                                {
                                    fragmentIdentifiers.Add(match.Groups[1].Value.Trim().ToLower());
                                }
                            }
                            _visitedLinks.Add(baseLink, fragmentIdentifiers.Count == 0 ? null : fragmentIdentifiers);

                            if (fragmentIdentifier != null && !fragmentIdentifiers.Contains(fragmentIdentifier))
                            {
                                ReportMissingFragmentIdentifier(fragmentIdentifier);
                            }
                            return true;
                        }
                        else if (statusCode.IsInRange(300, 399))
                        {
                            Uri redirectUri = response.Headers.Location;
                            if (!redirectUri.IsAbsoluteUri)
                            {
                                redirectUri = new Uri(request.RequestUri.GetLeftPart(UriPartial.Authority) + redirectUri);
                            }

                            if (TestLinkInternal(redirectUri.AbsoluteUri))
                            {
                                _visitedLinks.Add(baseLink, null);
                                return true;
                            }
                        }
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }

            void PrintRedirectChain()
            {
                Console.Write("Redirect chain: ");
                Console.WriteLine(string.Join($" => {Environment.NewLine}\t\t", redirectQueue.Select(request => $"`{request}`")));
            }

            if (TestLinkInternal(link))
            {
                if (redirectQueue.Count > 1) // Warning about redirect chain
                {
                    LinkWarning("returned a redirect chain");
                    PrintRedirectChain();
                }
            }
            else
            {
                if (redirectQueue.Count == MaxDepth) // Too many redirects
                {
                    LinkError("returned a too-long redirect chain");
                    PrintRedirectChain();
                }
                else // Broken link
                {
                    LinkError("appears to be broken");
                }
            }
        }

        private static List<string> GetAllFiles(string path, out List<string> allDirectories)
        {
            List<string> files = new List<string>();
            allDirectories = new List<string>();
            allDirectories.Add(path);
            int i = -1;
            while (++i < allDirectories.Count)
            {
                string directory = allDirectories[i];
                files.AddRange(Directory.GetFiles(directory));
                allDirectories.AddRange(Directory.GetDirectories(directory));
            }
            return files;
        }
        private static string CombineFilePaths(string basePath, string relative)
        {
            if (relative.StartsWith("./"))
                relative = relative.Substring(2);

            string baseDirectory = Path.GetDirectoryName(basePath).Replace('\\', '/');
            if (baseDirectory.Length == 0) return relative;
            string relativeDirectory = Path.GetDirectoryName(relative).Replace('\\', '/');
            string relativeFilename = Path.GetFileName(relative);

            while (relativeDirectory.StartsWith("../"))
            {
                relativeDirectory = relativeDirectory.Substring(3);
                if (baseDirectory.Length == 0) return "!invalid_path!";
                baseDirectory = Path.GetDirectoryName(baseDirectory).Replace('\\', '/');
            }

            return (baseDirectory.Length == 0 ? "" : baseDirectory + '/') +
                (relativeDirectory.Length == 0 ? "" : relativeDirectory + '/') + relativeFilename;
        }
    }
}
