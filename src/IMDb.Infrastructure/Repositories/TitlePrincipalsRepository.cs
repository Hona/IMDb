using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMDb.Core.Models;
using IMDb.Core.Repositories;
using Marten;

namespace IMDb.Infrastructure.Repositories
{
    public class TitlePrincipalsRepository : AbstractRepository<TitlePrincipals>, ITitlePrincipalsRepository
    {
        public TitlePrincipalsRepository(IDocumentStore store) : base(store) {}

        public async Task<IReadOnlyList<TitlePrincipals>> FindByTitle(string tconst)
        {
            using var session = _store.QuerySession();

            return await session.Query<TitlePrincipals>().Where(x => x.TConst == tconst)
                .ToListAsync();
        }

        public async Task<TitlePrincipals> FindByFullInfo(string tconst, string nconst)
        {
            using var session = _store.QuerySession();

            return await session.Query<TitlePrincipals>()
                .SingleOrDefaultAsync(x => x.TConst == tconst && x.NConst == nconst);
        }

        public async Task<IReadOnlyList<TitlePrincipals>> FindByOrdering(int page, int ordering)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitlePrincipals>().Where(x => x.Ordering == ordering)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitlePrincipals>> FindByPerson(int page, string nconst)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitlePrincipals>().Where(x => x.NConst == nconst)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitlePrincipals>> FindByCategory(int page, string category)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitlePrincipals>().Where(x =>
                    string.Equals(x.Category, category, StringComparison.InvariantCultureIgnoreCase))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitlePrincipals>> FindByJobTitle(int page, string jobTitle)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitlePrincipals>().Where(x =>
                    string.Equals(x.Job, jobTitle, StringComparison.InvariantCultureIgnoreCase))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitlePrincipals>> FindByCharacter(int page, string character)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitlePrincipals>().Where(x =>
                    string.Equals(x.Character, character, StringComparison.InvariantCultureIgnoreCase))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        protected override string GetUrl()
        {
            return "https://datasets.imdbws.com/title.principals.tsv.gz";
        }
    }
}