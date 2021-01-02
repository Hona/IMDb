using System.Collections.Generic;
using System.Threading.Tasks;
using IMDb.Core.Models;

namespace IMDb.Core.Repositories
{
    public interface ITitleEpisodeRepository : IRepository<TitleEpisode>
    {
        Task<TitleEpisode> FindByTConst(string tconst);
        Task<TitleEpisode> FindByFullInfo(string parentTconst, int? season, int? episode);

        Task<IReadOnlyList<TitleEpisode>> FindByParentTConst(int page, string tconst);
        Task<IReadOnlyList<TitleEpisode>> FindBySeason(int page, int season);
        Task<IReadOnlyList<TitleEpisode>> FindByEpisode(int page, int episode);
    }
}