using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMDb.Core.Models;
using IMDb.Core.Repositories;
using Marten;

namespace IMDb.Infrastructure.Repositories
{
    public class TitleCrewRepository : AbstractRepository<TitleCrew>, ITitleCrewRepository
    {
        public TitleCrewRepository(IDocumentStore store) : base(store) { }

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

        protected override string GetUrl() => "https://datasets.imdbws.com/title.crew.tsv.gz";
    }
}