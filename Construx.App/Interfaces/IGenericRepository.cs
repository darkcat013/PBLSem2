using Construx.App.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Construx.App.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetById(long id);

        Task<List<TEntity>> GetAll();

        Task SaveChangesAsync();

        void Add(TEntity entity);

        Task<TEntity> Delete(long id);

        Task<int> Count();
    }
}
