using System;
using System.Collections.Generic;
using System.IO;
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
                        if (!TestLink(reference))
                            ReportBrokenLink(reference, markdownFile.RelativePath, lineNr);
                    }
                    else // File reference
                    {
                        string referencedFilePath = CombineFilePaths(markdownFile.RelativeLowercasePath, reference.ToLower());

                        if (!markdownEntities.Contains(referencedFilePath))
                            ReportUnresolvedReference(reference, markdownFile.RelativePath, lineNr);
                    }
                }
            }

            if (ErrorsFound)
            {
                Environment.ExitCode = 1;
            }
            else
            {
                Console.WriteLine("All references are good!");
            }
        }

        private static bool ErrorsFound = false;
        private static void ReportError(string message)
        {
            Console.WriteLine("Markdown error: " + message);
            ErrorsFound = true;
        }
        private static void ReportFragmentError(string fragment, string file, int line, string message)
            => ReportError($"Fragment reference `{fragment}` in `{file}` on line {line} {message}.");
        private static void ReportUnresolvedReference(string reference, string file, int line)
            => ReportError($"Unresolved reference `{reference}` in `{file}` on line {line}.");
        private static void ReportBrokenLink(string link, string file, int line)
            => ReportError($"Broken link `{link}` in `{file}` on line {line}.");

        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly HashSet<string> _visitedLinks = new HashSet<string>();
        private static bool TestLink(string link)
        {
            try
            {
                string baseLink = (link.Contains('#') ? link.SubstringBefore('#') : link).TrimEnd('/');
                if (_visitedLinks.Contains(baseLink))
                {
                    Console.WriteLine("Skipping link: " + link);
                    return true;
                }
                Console.WriteLine("Testing link: " + link);
                var response = _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, link), HttpCompletionOption.ResponseHeadersRead).Result;
                bool successful = response.IsSuccessStatusCode;
                response.Dispose();
                _visitedLinks.Add(baseLink);
                return successful;
            }
            catch
            {
                return false;
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
