using IMDb.Core.Models;
using IMDb.Core.Parsers.Helpers;

namespace IMDb.Core.Parsers.Models
{
    public class TitleBasicsParser : IParser<TitleBasics>
    {
        public TitleBasics Parse(string line)
        {
            var parts = line.SplitOnTabs().ToNormalStrings();

            parts.EnsureCorrectPartLength<TitleBasics>();

            return new TitleBasics
            {
                TConst = parts[0],
                TitleType = parts[1],
                PrimaryTitle = parts[2],
                OriginalTitle = parts[3],
                IsAdult = parts[4]?.Contains("1"),
                StartYear = parts[5] == null ? null : int.Parse(parts[5]),
                EndYear = parts[6] == null ? null : int.Parse(parts[6]),
                RuntimeMinutes = parts[7] == null ? null : int.Parse(parts[7]),
                Genres = parts[8].SplitEnumeratedArray()
            };
        }
    }
}