using System.Collections.Generic;
using System.Threading.Tasks;
using IMDb.Core.Models;

namespace IMDb.Core.Repositories
{
    public interface ITitleRatingsRepository
    {
        Task Add(TitleRatings model);
        Task Update(TitleRatings model);
        Task Delete(TitleRatings model);
        void BulkSync(IEnumerable<TitleRatings> models);

        Task<TitleRatings> FindByTitle(string tconst);

        Task<IReadOnlyList<TitleRatings>> FindByRating(int page, decimal rating, decimal deltaAllowed = 0);
        Task<IReadOnlyList<TitleRatings>> FindAboveRating(int page, decimal rating);
        Task<IReadOnlyList<TitleRatings>> FindBelowRating(int page, decimal rating);
    }
}