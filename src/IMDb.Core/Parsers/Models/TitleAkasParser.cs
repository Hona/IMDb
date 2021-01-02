using IMDb.Core.Models;
using IMDb.Core.Parsers.Helpers;

namespace IMDb.Core.Parsers.Models
{
    public class TitleAkasParser : IParser<TitleAKAs>
    {
        public TitleAKAs Parse(string line)
        {
            var parts = line.SplitOnTabs().ToNormalStrings();

            parts.EnsureCorrectPartLength<TitleAKAs>();

            return new TitleAKAs
            {
                TitleId = parts[0],
                Ordering = int.Parse(parts[1]),
                Title = parts[2],
                Region = parts[3],
                Language = parts[4],
                Types = parts[5].SplitEnumeratedArray(),
                Attributes = parts[6].SplitNonEnumeratedArray(),
                IsOriginalTitle = parts[7]?.Contains("1")
            };
        }
    }
}