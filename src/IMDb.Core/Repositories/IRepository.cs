using System.Collections.Generic;
using System.Threading.Tasks;
using IMDb.Core.Models;

namespace IMDb.Core.Repositories
{
    public interface IRepository<T>
    {
        Task Add(T model);
        Task Update(T model);
        Task Delete(T model);
        void BulkSync(IEnumerable<T> models);
    }
}