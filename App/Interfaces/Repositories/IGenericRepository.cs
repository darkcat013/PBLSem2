using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Interfaces.Repositories
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
