using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMDb.Core.Models;
using IMDb.Core.Repositories;
using Marten;

namespace IMDb.Infrastructure.Repositories
{
    public class NameBasicsRepository : INameBasicsRepository
    {
        private readonly IDocumentStore _store;

        public NameBasicsRepository(IDocumentStore store) => _store = store;

        public async Task Add(NameBasics model)
        {
            using var session = _store.DirtyTrackedSession();

            session.Insert(model);

            await session.SaveChangesAsync();
        }

        public async Task Update(NameBasics model)
        {
            using var session = _store.DirtyTrackedSession();

            session.Update(model);

            await session.SaveChangesAsync();
        }

        public async Task Delete(NameBasics model)
        {
            using var session = _store.DirtyTrackedSession();

            session.Delete(model);

            await session.SaveChangesAsync();
        }

        public void BulkSync(IEnumerable<NameBasics> models)
        {
            _store.BulkInsert(models.ToArray(), BulkInsertMode.OverwriteExisting);
        }

        public async Task<NameBasics> FindByNConst(string nconst)
        {
            using var session = _store.QuerySession();

            return await session.Query<NameBasics>().SingleOrDefaultAsync(x => x.NConst == nconst);
        }

        public async Task<IReadOnlyList<NameBasics>> FindByPrimaryName(int page, string primaryName)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<NameBasics>().Where(x =>
                    string.Equals(x.PrimaryName, primaryName, StringComparison.InvariantCultureIgnoreCase))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<NameBasics>> FindByBirthYear(int page, int? year)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<NameBasics>().Where(x => x.BirthYear == year)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<NameBasics>> FindByDeathYear(int page, int? year)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<NameBasics>().Where(x => x.DeathYear == year)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<NameBasics>> FindByPrimaryProfessions(int page, params string[] professions)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<NameBasics>().Where(x => professions.All(y => x.PrimaryProfession.Contains(y)))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<NameBasics>> FindByTitle(int page, string tconst)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<NameBasics>().Where(x => x.KnownForTitles.Contains(tconst))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }
    }
}