using System.Collections.Generic;
using System.Linq;

namespace IMDb.Core.Parsers.Helpers
{
    public static class TsvHelper
    {
        public static string[] SplitEnumeratedArray(this string array) => array?.Split(",");
        public static string[] SplitNonEnumeratedArray(this string array) => array?.Split(" ");
        public static string[] SplitOnTabs(this string line) => line.Split("\t");

        public static string[] ToNormalStrings(this IEnumerable<string> rawParts) =>
            rawParts.Select(x => x.ToNormalString()).ToArray();

        public static bool IsImbdNull(this string value) => value.Equals(@"\N");
        public static string ToNormalString(this string value) => IsImbdNull(value) ? null : value;
    }
}