using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneDataAccess.Repository
{
    public class Repository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly DatabaseConnection _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DatabaseConnection dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }
        public void Create(TEntity entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }

        public void DeleteRange(List<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            _dbContext.SaveChanges();   
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }


        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
            _dbContext.SaveChanges();
        }

        public IQueryable<TEntity> Queryable()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<int> Save()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }

}
