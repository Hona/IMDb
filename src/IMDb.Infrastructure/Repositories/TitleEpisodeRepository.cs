using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMDb.Core.Models;
using IMDb.Core.Repositories;
using Marten;

namespace IMDb.Infrastructure.Repositories
{
    public class TitleEpisodeRepository : ITitleEpisodeRepository
    {
        private readonly IDocumentStore _store;

        public TitleEpisodeRepository(IDocumentStore store) => _store = store;

        public async Task Add(TitleEpisode model)
        {
            using var session = _store.DirtyTrackedSession();

            session.Insert(model);

            await session.SaveChangesAsync();
        }

        public async Task Update(TitleEpisode model)
        {
            using var session = _store.DirtyTrackedSession();

            session.Update(model);

            await session.SaveChangesAsync();
        }

        public async Task Delete(TitleEpisode model)
        {
            using var session = _store.DirtyTrackedSession();

            session.Delete(model);

            await session.SaveChangesAsync();
        }

        public void BulkSync(IEnumerable<TitleEpisode> models)
        {
            _store.BulkInsert(models.ToArray(), BulkInsertMode.OverwriteExisting);
        }

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
    }
}