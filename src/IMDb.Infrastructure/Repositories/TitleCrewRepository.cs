using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMDb.Core.Models;
using IMDb.Core.Repositories;
using Marten;

namespace IMDb.Infrastructure.Repositories
{
    public class TitleCrewRepository : ITitleCrewRepository
    {
        private readonly IDocumentStore _store;

        public TitleCrewRepository(IDocumentStore store) => _store = store;

        public async Task Add(TitleCrew model)
        {
            using var session = _store.DirtyTrackedSession();

            session.Insert(model);

            await session.SaveChangesAsync();
        }

        public async Task Update(TitleCrew model)
        {
            using var session = _store.DirtyTrackedSession();

            session.Update(model);

            await session.SaveChangesAsync();
        }

        public async Task Delete(TitleCrew model)
        {
            using var session = _store.DirtyTrackedSession();

            session.Delete(model);

            await session.SaveChangesAsync();
        }

        public void BulkSync(IEnumerable<TitleCrew> models)
        {
            _store.BulkInsert(models.ToArray(), BulkInsertMode.OverwriteExisting);
        }

        public async Task<TitleCrew> FindByTConst(string tconst)
        {
            using var session = _store.QuerySession();

            return await session.Query<TitleCrew>().SingleOrDefaultAsync(x => x.TConst == tconst);
        }

        public async Task<IReadOnlyList<TitleCrew>> FindByDirectors(int page, params string[] directorsNconsts)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleCrew>().Where(x => directorsNconsts.All(y => x.Directors.Contains(y)))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleCrew>> FindByWriters(int page, params string[] writersNconsts)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleCrew>().Where(x => writersNconsts.All(y => x.Writers.Contains(y)))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }
    }
}