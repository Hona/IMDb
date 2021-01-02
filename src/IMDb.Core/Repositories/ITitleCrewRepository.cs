using System.Collections.Generic;
using System.Threading.Tasks;
using IMDb.Core.Models;

namespace IMDb.Core.Repositories
{
    public interface ITitleCrewRepository
    {
        Task Add(TitleCrew model);
        Task Update(TitleCrew model);
        Task Delete(TitleCrew model);
        void BulkSync(IEnumerable<TitleCrew> models);

        Task<TitleCrew> FindByTConst(string tconst);

        Task<IReadOnlyList<TitleCrew>> FindByDirectors(int page, params string[] directorsNconsts);
        Task<IReadOnlyList<TitleCrew>> FindByWriters(int page, params string[] writersNconsts);
    }
}