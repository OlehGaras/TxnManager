using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TxnManager.Data.Abstractions;

namespace TxnManager.Data.EntityFramework
{
    public class Repository<T>: IRepository<T> where T: class
    {
        private readonly TransactionsManagerDbContext _dbContext;

        public Repository(TransactionsManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> All()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public IQueryable<T> All(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate).AsQueryable();
        }

        public async Task<T> FindAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            var entry = await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRange(List<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            _dbContext.SaveChanges();
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = await FindAsync(id);

            if (entity == null)
            {
                throw new ArgumentException($"The entity of type {typeof(T).FullName} with an id {id} was not found!");
            }

            Delete(entity);
        }
    }
}