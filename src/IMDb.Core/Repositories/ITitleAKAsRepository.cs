using System.Collections.Generic;
using System.Threading.Tasks;
using IMDb.Core.Models;

namespace IMDb.Core.Repositories
{
    public interface ITitleAKAsRepository : IRepository<TitleAKAs>
    {
        Task<TitleAKAs> FindByFullInfo(string tconst, int ordering);
        Task<IReadOnlyList<TitleAKAs>> FindByTitleId(string tconst);

        Task<IReadOnlyList<TitleAKAs>> FindByTitle(int page, string title);
        Task<IReadOnlyList<TitleAKAs>> FindByRegion(int page, string region);
        Task<IReadOnlyList<TitleAKAs>> FindByLanguage(int page, string language);
        Task<IReadOnlyList<TitleAKAs>> FindByTypes(int page, params string[] types);
        Task<IReadOnlyList<TitleAKAs>> FindByAttributes(int page, params string[] attributes);
        Task<IReadOnlyList<TitleAKAs>> FindByIsOriginalTitle(int page, bool? isOriginalTitle);
    }
}