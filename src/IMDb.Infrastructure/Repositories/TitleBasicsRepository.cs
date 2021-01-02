using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMDb.Core.Models;
using IMDb.Core.Repositories;
using Marten;

namespace IMDb.Infrastructure.Repositories
{
    public class TitleBasicsRepository : AbstractRepository<TitleBasics>, ITitleBasicsRepository
    {
        public TitleBasicsRepository(IDocumentStore store) : base(store) { }

        public async Task<TitleBasics> FindByTConst(string tconst)
        {
            using var session = _store.QuerySession();

            return await session.Query<TitleBasics>().SingleOrDefaultAsync(x => x.TConst == tconst);
        }

        public async Task<IReadOnlyList<TitleBasics>> FindByTitleType(int page, string type)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleBasics>().Where(x =>
                    string.Equals(x.TitleType, type, StringComparison.InvariantCultureIgnoreCase))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleBasics>> FindByPrimaryTitle(int page, string primaryTitle)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleBasics>().Where(x =>
                    string.Equals(x.PrimaryTitle, primaryTitle, StringComparison.InvariantCultureIgnoreCase))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleBasics>> FindByOriginalTitle(int page, string originalTitle)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleBasics>().Where(x =>
                    string.Equals(x.OriginalTitle, originalTitle, StringComparison.InvariantCultureIgnoreCase))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleBasics>> FindByIsAdult(int page, bool? isAdult)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleBasics>().Where(x => x.IsAdult == isAdult)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleBasics>> FindByStartYear(int page, int startYear)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleBasics>().Where(x => x.StartYear == startYear)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleBasics>> FindByEndYear(int page, int endYear)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleBasics>().Where(x => x.EndYear == endYear)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleBasics>> FindByRuntimeMinutes(int page, int runtime, int deltaAllowed = 0)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleBasics>().Where(x =>
                    x.RuntimeMinutes >= runtime - deltaAllowed && x.RuntimeMinutes <= runtime + deltaAllowed)
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TitleBasics>> FindByGenres(int page, params string[] genres)
        {
            using var session = _store.QuerySession();

            var pagination = page.GetPaginationData();

            return await session.Query<TitleBasics>().Where(x => genres.All(y => x.Genres.Contains(y)))
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();
        }
    }
}