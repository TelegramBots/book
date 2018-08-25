using System;

namespace LinkValidator
{
    static class Helpers
    {
        public static bool OrdinalContains(this string source, string value)
            => source.OrdinalIndexOf(value) != -1;

        public static bool Contains(this string source, char value)
            => source.IndexOf(value) != -1;

        public static int OrdinalIndexOf(this string source, string value, int offset = 0)
            => source.IndexOf(value, offset, StringComparison.Ordinal);

        public static string SubstringAfter(this string source, string value, int offset = 0)
        {
            int index = source.OrdinalIndexOf(value, offset);
            if (index < 0) throw new ArgumentException("Value is not a substring of source");
            return source.Substring(index + value.Length);
        }

        public static string SubstringBefore(this string source, char value, int offset = 0)
        {
            int index = source.IndexOf(value, offset);
            if (index < 0) throw new ArgumentException("Value is not a substring of source");
            return source.Substring(offset, index - offset);
        }
    }
}
