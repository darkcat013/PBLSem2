using App.Domain.Entities;
using App.EF;
using App.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ConstruxDbContext _testDbContext;

        public GenericRepository(ConstruxDbContext testDbContext)
        {
            _testDbContext = testDbContext;
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _testDbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetById(long id)
        {
            return await _testDbContext.FindAsync<TEntity>(id);
        }

        public async Task SaveChangesAsync()
        {
            await _testDbContext.SaveChangesAsync();
        }

        public void Add(TEntity entity)
        {
            _testDbContext
                .Set<TEntity>()
                .Add(entity);
        }

        public async Task<TEntity> Delete(long id)
        {
            var entity = await _testDbContext.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                throw new Exception($"Object of type {typeof(TEntity)} with id { id } not found");
            }

            _testDbContext.Set<TEntity>().Remove(entity);

            return entity;
        }

        public async Task<int> Count()
        {
            return await _testDbContext.Set<TEntity>().CountAsync();
        }
    }
}
