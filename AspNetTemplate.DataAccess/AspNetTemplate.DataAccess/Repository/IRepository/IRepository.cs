using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetTemplate.DataAccess.Repository.IRepository
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        Task<IEnumerable<TEntity>> AllAsync();

        Task<TEntity> FindAsync(TKey key);

        Task AddAsync(TEntity entity); 

        Task UpdateAsync(TEntity entity);

        Task RemoveAsync(TKey key);
    }
}
