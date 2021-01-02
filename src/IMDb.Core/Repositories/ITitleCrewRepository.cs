using System.Collections.Generic;
using System.Threading.Tasks;
using IMDb.Core.Models;

namespace IMDb.Core.Repositories
{
    public interface ITitleCrewRepository : IRepository<TitleCrew>
    {
        Task<TitleCrew> FindByTConst(string tconst);

        Task<IReadOnlyList<TitleCrew>> FindByDirectors(int page, params string[] directorsNconsts);
        Task<IReadOnlyList<TitleCrew>> FindByWriters(int page, params string[] writersNconsts);
    }
}