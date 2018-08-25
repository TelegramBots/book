using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace LinkValidator
{
    public class MarkdownFile
    {
        public readonly string FullPath;
        public readonly string RelativePath;
        public readonly string RelativeLowercasePath;
        public readonly HashSet<string> HeaderEntities = new HashSet<string>();
        public readonly List<(int lineNumber, string reference)> References = new List<(int, string)>();

        public MarkdownFile(string fullPath, string relativePath, string relativeLowercasePath)
        {
            FullPath = fullPath;
            RelativePath = relativePath;
            RelativeLowercasePath = relativeLowercasePath;
        }        
        
        // First group matches inline references, second group matches named references, third group matches headers
        // See https://regex101.com/r/66Xj8g/4
        private static readonly Regex MarkdownRegex = new Regex(@"\[.*?\]\((.*?)\)|\[.*?\]: (.*?)$|^(#.*?$)", RegexOptions.Compiled);

        public void Parse()
        {
            string[] markdownSource = File.ReadAllLines(FullPath);

            for (int lineNr = 0; lineNr < markdownSource.Length; lineNr++)
            {
                foreach (Match match in MarkdownRegex.Matches(markdownSource[lineNr]))
                {
                    if (match.Groups[1].Success) References.Add((lineNr + 1, match.Groups[1].Value.Trim()));
                    if (match.Groups[2].Success) References.Add((lineNr + 1, match.Groups[2].Value.Trim()));
                    if (match.Groups[3].Success) HeaderEntities.Add(GetHeaderEntity(match.Groups[3].Value.Trim()));
                }
            }
        }

        private static string GetHeaderEntity(string header)
        {
            header = header
                .TrimStart('#')
                .Replace(' ', '-')
                .ToLower();

            int length;
            do
            {
                length = header.Length;
                header = header.Replace("  ", " ");
            }
            while (length != header.Length);

            return '#' + header.Trim('-');
        }
    }
}
