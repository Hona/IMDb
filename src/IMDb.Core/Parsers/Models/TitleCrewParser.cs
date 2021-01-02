using IMDb.Core.Models;
using IMDb.Core.Parsers.Helpers;

namespace IMDb.Core.Parsers.Models
{
    public class TitleCrewParser : IParser<TitleCrew>
    {
        public TitleCrew Parse(string line)
        {
            var parts = line.SplitOnTabs().ToNormalStrings();

            parts.EnsureCorrectPartLength<TitleCrew>();

            return new TitleCrew
            {
                TConst = parts[0],
                Directors = parts[1].SplitEnumeratedArray(),
                Writers = parts[2].SplitEnumeratedArray()
            };
        }
    }
}