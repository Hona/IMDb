using System.Collections.Generic;
using System.Threading.Tasks;
using IMDb.Core.Models;

namespace IMDb.Core.Repositories
{
    public interface INameBasicsRepository : IRepository<NameBasics>
    {
        Task<NameBasics> FindByNConst(string nconst);

        Task<IReadOnlyList<NameBasics>> FindByPrimaryName(int page, string primaryName);
        Task<IReadOnlyList<NameBasics>> FindByBirthYear(int page, int? year);
        Task<IReadOnlyList<NameBasics>> FindByDeathYear(int page, int? year);
        Task<IReadOnlyList<NameBasics>> FindByPrimaryProfessions(int page, params string[] professions);
        Task<IReadOnlyList<NameBasics>> FindByTitle(int page, string tconst);
    }
}