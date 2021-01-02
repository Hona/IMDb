using IMDb.Core.Models;
using IMDb.Core.Parsers.Helpers;

namespace IMDb.Core.Parsers.Models
{
    public class TitlePrincipalsParser : IParser<TitlePrincipals>
    {
        public TitlePrincipals Parse(string line)
        {
            var parts = line.SplitOnTabs().ToNormalStrings();

            parts.EnsureCorrectPartLength<TitlePrincipals>();

            return new TitlePrincipals
            {
                TConst = parts[0],
                Ordering = int.Parse(parts[1]),
                NConst = parts[2],
                Category = parts[3],
                Job = parts[4],
                Character = parts[5]
            };
        }
    }
}