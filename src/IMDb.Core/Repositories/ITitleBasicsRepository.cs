using System.Collections.Generic;
using System.Threading.Tasks;
using IMDb.Core.Models;

namespace IMDb.Core.Repositories
{
    public interface ITitleBasicsRepository : IRepository<TitleBasics>
    {
        Task<TitleBasics> FindByTConst(string tconst);

        Task<IReadOnlyList<TitleBasics>> FindByTitleType(int page, string type);
        Task<IReadOnlyList<TitleBasics>> FindByPrimaryTitle(int page, string primaryTitle);
        Task<IReadOnlyList<TitleBasics>> FindByOriginalTitle(int page, string originalTitle);
        Task<IReadOnlyList<TitleBasics>> FindByIsAdult(int page, bool? isAdult);
        Task<IReadOnlyList<TitleBasics>> FindByStartYear(int page, int startYear);
        Task<IReadOnlyList<TitleBasics>> FindByEndYear(int page, int endYear);
        Task<IReadOnlyList<TitleBasics>> FindByRuntimeMinutes(int page, int runtime, int deltaAllowed = 0);
        Task<IReadOnlyList<TitleBasics>> FindByGenres(int page, params string[] genres);
    }
}