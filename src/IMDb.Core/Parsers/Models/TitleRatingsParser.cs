using IMDb.Core.Models;
using IMDb.Core.Parsers.Helpers;

namespace IMDb.Core.Parsers.Models
{
    public class TitleRatingsParser : IParser<TitleRatings>
    {
        public TitleRatings Parse(string line)
        {
            var parts = line.SplitOnTabs().ToNormalStrings();

            parts.EnsureCorrectPartLength<TitleRatings>();

            return new TitleRatings
            {
                TConst = parts[0],
                AverageRating = decimal.Parse(parts[1]),
                TotalVotes = int.Parse(parts[2])
            };
        }
    }
}