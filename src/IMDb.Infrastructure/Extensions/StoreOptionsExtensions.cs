using IMDb.Core.Models;
using Marten;

namespace IMDb.Infrastructure.Extensions
{
    public static class StoreOptionsExtensions
    {
        public static void SetupIMDbIdentities(this StoreOptions options)
        {
            options.Schema.For<NameBasics>().Identity(x => x.NConst);
            options.Schema.For<TitleAKAs>().Identity(x => x.MartenId);
            options.Schema.For<TitleBasics>().Identity(x => x.TConst);
            options.Schema.For<TitleCrew>().Identity(x => x.TConst);
            options.Schema.For<TitleEpisode>().Identity(x => x.MartenId);
            options.Schema.For<TitlePrincipals>().Identity(x => x.MartenId);
            options.Schema.For<TitleRatings>().Identity(x => x.TConst);
        }
    }
}