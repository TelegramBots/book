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

        public readonly List<(int lineNumber, string referenceName)> DefinedReferenceNames = new List<(int, string)>();
        public readonly List<(int lineNumber, string referenceName)> AccessedReferenceNames = new List<(int, string)>();

        public MarkdownFile(string fullPath, string relativePath, string relativeLowercasePath)
        {
            FullPath = fullPath;
            RelativePath = relativePath;
            RelativeLowercasePath = relativeLowercasePath;
        }

        // See https://regex101.com/r/66Xj8g/7
        // There are 7 capturing groups
        // 1: Inline references - [Test](link) - 'link' is captured
        // 2: Named reference name - [Test]: link - 'Test' is captured
        // 3: Named reference target - [Test]: link - 'link' is captured
        // 4: Headers - # header - '# header' is captured
        // 5: Inline named reference name - [Test][some link] - 'some link' is captured
        // 6: Same-name named references - [Test] - 'Test' is captured
        // 7: Numbered reference definition - [^1]: ... -  '^1' is captured
        private static readonly Regex MarkdownRegex =
            new Regex(
                @"\[[^\[\]]*?\]\((.*?)\)|\[(.*?)\]: ([^\[\]]*?)$|^(#.*?$)|\[.*?\]\[(.*?)\]|(?:[^\]]|^)\[([^\[\]]*?)\](?:[^\[\(:]|$)|\[(\^.*?)\]:",
                RegexOptions.Compiled);

        private static readonly Regex InlineMonoTextRegex = new Regex(@"[^\[]?`[^\]].*?[^\[]`[^\]]", RegexOptions.Compiled);

        public void Parse()
        {
            string[] markdownSource = File.ReadAllLines(FullPath);

            bool codeBlock = false;
            for (int lineNr = 1; lineNr <= markdownSource.Length; lineNr++)
            {
                string lineSource = markdownSource[lineNr - 1];
                if (lineSource.StartsWith("```"))
                    codeBlock = !codeBlock;
                if (codeBlock) continue;

                lineSource = InlineMonoTextRegex.Replace(lineSource, "");

                foreach (Match match in MarkdownRegex.Matches(lineSource))
                {
                    if (match.Groups[1].Success) References.Add((lineNr, match.Groups[1].Value.Trim()));
                    if (match.Groups[3].Success) References.Add((lineNr, match.Groups[3].Value.Trim()));
                    if (match.Groups[4].Success) HeaderEntities.Add(GetHeaderEntity(match.Groups[4].Value.Trim()));

                    if (match.Groups[2].Success) DefinedReferenceNames.Add((lineNr, match.Groups[2].Value));
                    if (match.Groups[7].Success) DefinedReferenceNames.Add((lineNr, match.Groups[7].Value));

                    if (match.Groups[5].Success) AccessedReferenceNames.Add((lineNr, match.Groups[5].Value));
                    if (match.Groups[6].Success) AccessedReferenceNames.Add((lineNr, match.Groups[6].Value));
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
