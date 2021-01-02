using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMDb.Core.Models;
using IMDb.Core.Repositories;
using Marten;

namespace IMDb.Infrastructure.Repositories
{
    public class TitleRatingsRepository : ITitleRatingsRepository
    {
        private readonly IDocumentStore _store;

        public TitleRatingsRepository(IDocumentStore store) => _store = store;

        public async Task Add(TitleRatings model)
        {
            using var session = _store.DirtyTrackedSession();

            session.Insert(model);

            await session.SaveChangesAsync();
        }

        public async Task Update(TitleRatings model)
        {
            using var session = _store.DirtyTrackedSession();

            session.Update(model);

            await session.SaveChangesAsync();
        }

        public async Task Delete(TitleRatings model)
        {
            using var session = _store.DirtyTrackedSession();

            session.Delete(model);

            await session.SaveChangesAsync();
        }

        public void BulkSync(IEnumerable<TitleRatings> models)
        {
            _store.BulkInsert(models.ToArray(), BulkInsertMode.OverwriteExisting);
        }

        public async Task<TitleRatings> FindByTitle(string tconst)
        {
            using var session = _store.QuerySession();

            return await session.Query<TitleRatings>().SingleOrDefaultAsync(x => x.TConst == tconst);
        }

        public async Task<IReadOnlyList<TitleRatings>> FindByRating(int page, decimal rating, decimal deltaAllowed = 0)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleRatings>().Where(x =>
                    x.AverageRating >= rating - deltaAllowed && x.AverageRating <= rating + deltaAllowed)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleRatings>> FindAboveRating(int page, decimal rating)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleRatings>().Where(x => x.AverageRating > rating)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleRatings>> FindBelowRating(int page, decimal rating)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleRatings>().Where(x => x.AverageRating < rating)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }
    }
}