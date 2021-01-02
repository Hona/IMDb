using System.Collections.Generic;
using System.Threading.Tasks;
using IMDb.Core.Models;

namespace IMDb.Core.Repositories
{
    public interface INameBasicsRepository
    {
        Task Add(NameBasics model);
        Task Update(NameBasics model);
        Task Delete(NameBasics model);
        void BulkSync(IEnumerable<NameBasics> models);

        Task<NameBasics> FindByNConst(string nconst);

        Task<IReadOnlyList<NameBasics>> FindByPrimaryName(int page, string primaryName);
        Task<IReadOnlyList<NameBasics>> FindByBirthYear(int page, int? year);
        Task<IReadOnlyList<NameBasics>> FindByDeathYear(int page, int? year);
        Task<IReadOnlyList<NameBasics>> FindByPrimaryProfessions(int page, params string[] professions);
        Task<IReadOnlyList<NameBasics>> FindByTitle(int page, string tconst);
    }
}