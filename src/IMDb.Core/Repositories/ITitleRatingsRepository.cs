using System.Collections.Generic;
using System.Threading.Tasks;
using IMDb.Core.Models;

namespace IMDb.Core.Repositories
{
    public interface ITitleRatingsRepository : IRepository<TitleRatings>
    {
        Task<TitleRatings> FindByTitle(string tconst);

        Task<IReadOnlyList<TitleRatings>> FindByRating(int page, decimal rating, decimal deltaAllowed = 0);
        Task<IReadOnlyList<TitleRatings>> FindAboveRating(int page, decimal rating);
        Task<IReadOnlyList<TitleRatings>> FindBelowRating(int page, decimal rating);
    }
}