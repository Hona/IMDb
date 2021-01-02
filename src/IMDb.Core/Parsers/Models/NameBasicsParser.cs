using IMDb.Core.Models;
using IMDb.Core.Parsers.Helpers;

namespace IMDb.Core.Parsers.Models
{
    public class NameBasicsParser : IParser<NameBasics>
    {
        public NameBasics Parse(string line)
        {
            var parts = line.SplitOnTabs().ToNormalStrings();

            parts.EnsureCorrectPartLength<NameBasics>();

            return new NameBasics
            {
                NConst = parts[0],
                PrimaryName = parts[1],
                BirthYear = parts[2] == null ? null : int.Parse(parts[2]),
                DeathYear = parts[3] == null ? null : int.Parse(parts[3]),
                PrimaryProfession = parts[4].SplitEnumeratedArray(),
                KnownForTitles = parts[5].SplitEnumeratedArray()
            };
        }
    }
}