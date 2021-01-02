using IMDb.Core.Models;
using IMDb.Core.Parsers.Helpers;

namespace IMDb.Core.Parsers.Models
{
    public class TitleEpisodeParser : IParser<TitleEpisode>
    {
        public TitleEpisode Parse(string line)
        {
            var parts = line.SplitOnTabs().ToNormalStrings();

            parts.EnsureCorrectPartLength<TitleEpisode>();

            return new TitleEpisode
            {
                TConst = parts[0],
                ParentTConst = parts[1],
                SeasonNumber = parts[2] == null ? null : int.Parse(parts[2]),
                EpisodeNumber = parts[3] == null ? null : int.Parse(parts[3])
            };
        }
    }
}