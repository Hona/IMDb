using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMDb.Core.Models;
using IMDb.Core.Repositories;
using Marten;

namespace IMDb.Infrastructure.Repositories
{
    public class TitleAKAsRepository : AbstractRepository<TitleAKAs>, ITitleAKAsRepository
    {
        public TitleAKAsRepository(IDocumentStore store) : base(store) { }

        public async Task<TitleAKAs> FindByFullInfo(string tconst, int ordering)
        {
            using var session = _store.QuerySession();

            return await session.Query<TitleAKAs>().SingleAsync(x => x.TitleId == tconst && x.Ordering == ordering);
        }

        public async Task<IReadOnlyList<TitleAKAs>> FindByTitleId(string tconst)
        {
            using var session = _store.QuerySession();

            return await session.Query<TitleAKAs>().Where(x => x.TitleId == tconst).ToListAsync();
        }

        public async Task<IReadOnlyList<TitleAKAs>> FindByTitle(int page, string title)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleAKAs>().Where(x =>
                    string.Equals(x.Title, title, StringComparison.InvariantCultureIgnoreCase))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleAKAs>> FindByRegion(int page, string region)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleAKAs>().Where(x =>
                    string.Equals(x.Region, region, StringComparison.InvariantCultureIgnoreCase))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleAKAs>> FindByLanguage(int page, string language)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleAKAs>().Where(x =>
                    string.Equals(x.Language, language, StringComparison.InvariantCultureIgnoreCase))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleAKAs>> FindByTypes(int page, params string[] types)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleAKAs>().Where(x => types.All(y => x.Types.Contains(y)))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleAKAs>> FindByAttributes(int page, params string[] attributes)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleAKAs>().Where(x => attributes.All(y => x.Attributes.Contains(y)))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleAKAs>> FindByIsOriginalTitle(int page, bool? isOriginalTitle)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleAKAs>().Where(x => x.IsOriginalTitle == isOriginalTitle)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        protected override string GetUrl() => "https://datasets.imdbws.com/title.akas.tsv.gz";
    }
}