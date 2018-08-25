using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace LinkValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            string srcDirectory = Environment.CurrentDirectory + @"\src\";
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
                            ReportFragmentError(reference, markdownFile.RelativePath, lineNr, "has double ##");
                            continue;
                        }

                        string fragment = reference.ToLower();
                        if (reference.StartsWith('#')) // Regular fragment (e.g. #usage)
                        {
                            if (!markdownFile.HeaderEntities.Contains(fragment))
                                ReportFragmentError(reference, markdownFile.RelativePath, lineNr, "could not be resolved");                                
                        }
                        else // Cross-file fragment (e.g. ./proxy.md#http-proxy
                        {
                            string markdownReference = fragment.SubstringBefore('#');
                            fragment = fragment.Substring(markdownReference.Length);

                            string referencedFilePath = CombineFilePaths(markdownFile.RelativeLowercasePath, markdownReference);

                            if (!markdownFiles.TryGetValue(referencedFilePath, out MarkdownFile referencedFile) ||
                                !referencedFile.HeaderEntities.Contains(fragment))
                                ReportFragmentError(reference, markdownFile.RelativePath, lineNr, "could not be resolved");
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
                            ReportUnresolvedReference(reference, markdownFile.RelativePath, lineNr);
                    }
                }

                HashSet<string> definedReferenceNames = new HashSet<string>(
                    markdownFile.DefinedReferenceNames
                    .Select(reference => reference.referenceName.Trim().ToLower()));
                foreach (var (lineNr, name) in markdownFile.AccessedReferenceNames)
                {
                    if (!definedReferenceNames.Contains(name.Trim().ToLower()))
                        ReportUndefinedNamedReference(name, markdownFile.RelativePath, lineNr);
                }

                HashSet<string> accessedReferenceNames = new HashSet<string>(
                    markdownFile.AccessedReferenceNames
                    .Select(reference => reference.referenceName.Trim().ToLower()));
                foreach (var (lineNr, name) in markdownFile.DefinedReferenceNames)
                {
                    if (!accessedReferenceNames.Contains(name.Trim().ToLower()))
                        ReportUnusedNamedReference(name, markdownFile.RelativePath, lineNr);
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

        private static bool ErrorsFound = false;
        private static bool WarningsFound = false;

        private static void PrintColor(string message, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = previousColor;
        }
        private static void ReportError(string message)
        {
            PrintColor("Markdown error: ", ConsoleColor.Red);
            Console.WriteLine(message);
            ErrorsFound = true;
        }
        private static void ReportWarning(string message)
        {
            PrintColor("Markdown warning: ", ConsoleColor.Red);
            Console.WriteLine(message);
            WarningsFound = true;
        }
        private static void ReportFragmentError(string fragment, string file, int line, string message)
            => ReportError($"Fragment reference `{fragment}` in `{file}` on line {line} {message}.");
        private static void ReportUnresolvedReference(string reference, string file, int line)
            => ReportError($"Unresolved reference `{reference}` in `{file}` on line {line}.");
        private static void ReportUndefinedNamedReference(string name, string file, int line)
            => ReportError($"Unresolved reference name `{name}` in `{file}` on line {line}.");
        private static void ReportUnusedNamedReference(string name, string file, int line)
            => ReportWarning($"Unused named reference: `{name}` in `{file}` on line {line}.");

        private static readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler() { AllowAutoRedirect = false });
        private static readonly HashSet<string> _visitedLinks = new HashSet<string>();
        private static void TestLink(string link, string sourceFile, int line)
        {
            Queue<string> redirectQueue = new Queue<string>();

            const int MaxDepth = 5;

            bool TestLinkInternal(string requestUri)
            {
                try
                {
                    redirectQueue.Enqueue(requestUri);
                    if (redirectQueue.Count == MaxDepth)
                        return false;

                    string baseLink = (requestUri.Contains('#') ? requestUri.SubstringBefore('#') : requestUri).TrimEnd('/');
                    if (_visitedLinks.Contains(baseLink))
                        return true;

                    using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
                    using (var response = _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result)
                    {
                        int statusCode = (int)response.StatusCode;
                        if (statusCode.IsInRange(200, 299))
                        {
                            _visitedLinks.Add(baseLink);
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
                                _visitedLinks.Add(baseLink);
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
            string LinkReferenceMessage(string message)
                => $"Link `{link}` in `{sourceFile}` on line {line} {message}.";

            if (TestLinkInternal(link))
            {
                if (redirectQueue.Count > 1) // Warning about redirect chain
                {
                    ReportWarning(LinkReferenceMessage("returned a redirect chain"));
                    PrintRedirectChain();
                }
            }
            else
            {
                if (redirectQueue.Count == MaxDepth) // Too many redirects
                {
                    ReportError(LinkReferenceMessage("returned a too-long redirect chain"));
                    PrintRedirectChain();
                }
                else // Broken link
                {
                    ReportError(LinkReferenceMessage("appears to be broken"));
                }
            }
        }

        private static List<string> GetAllFiles(string path, out List<string> allDirectories)
        {
            allDirectories = new List<string>();
            List<string> files = new List<string>();
            Stack<string> dirsToTraverse = new Stack<string>();
            dirsToTraverse.Push(path);
            while (dirsToTraverse.Count > 0)
            {
                string directory = dirsToTraverse.Pop();
                files.AddRange(Directory.GetFiles(directory));
                foreach (var dir in Directory.GetDirectories(directory))
                {
                    dirsToTraverse.Push(dir);
                    allDirectories.Add(dir);
                }
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
