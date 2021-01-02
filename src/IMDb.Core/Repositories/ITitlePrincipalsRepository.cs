using System.Collections.Generic;
using System.Threading.Tasks;
using IMDb.Core.Models;

namespace IMDb.Core.Repositories
{
    public interface ITitlePrincipalsRepository : IRepository<TitlePrincipals>
    {
        Task<IReadOnlyList<TitlePrincipals>> FindByTitle(string tconst);
        Task<TitlePrincipals> FindByFullInfo(string tconst, string nconst);

        Task<IReadOnlyList<TitlePrincipals>> FindByOrdering(int page, int ordering);
        Task<IReadOnlyList<TitlePrincipals>> FindByPerson(int page, string nconst);
        Task<IReadOnlyList<TitlePrincipals>> FindByCategory(int page, string category);
        Task<IReadOnlyList<TitlePrincipals>> FindByJobTitle(int page, string jobTitle);
        Task<IReadOnlyList<TitlePrincipals>> FindByCharacter(int page, string character);
    }
}