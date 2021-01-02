using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMDb.Core.Models;
using IMDb.Core.Repositories;
using Marten;

namespace IMDb.Infrastructure.Repositories
{
    public class TitleEpisodeRepository : AbstractRepository<TitleEpisode>, ITitleEpisodeRepository
    {
        public TitleEpisodeRepository(IDocumentStore store) : base(store) {}

        public async Task<TitleEpisode> FindByTConst(string tconst)
        {
            using var session = _store.QuerySession();

            return await session.Query<TitleEpisode>().SingleOrDefaultAsync(x => x.TConst == tconst);
        }

        public async Task<TitleEpisode> FindByFullInfo(string parentTconst, int? season, int? episode)
        {
            using var session = _store.QuerySession();

            return await session.Query<TitleEpisode>().SingleOrDefaultAsync(x =>
                x.ParentTConst == parentTconst && x.SeasonNumber == season && x.EpisodeNumber == episode);
        }

        public async Task<IReadOnlyList<TitleEpisode>> FindByParentTConst(int page, string tconst)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleEpisode>().Where(x => x.ParentTConst == tconst)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleEpisode>> FindBySeason(int page, int season)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleEpisode>().Where(x => x.SeasonNumber == season)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleEpisode>> FindByEpisode(int page, int episode)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleEpisode>().Where(x => x.EpisodeNumber == episode)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        protected override string GetUrl()
        {
            return "https://datasets.imdbws.com/title.episode.tsv.gz";
        }
    }
}