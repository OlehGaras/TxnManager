using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TxnManager.Data.Abstractions
{
    public interface IRepository<T>
    {
        IQueryable<T> All();

        IQueryable<T> All(Expression<Func<T, bool>> predicate);

        Task<T> FindAsync(Guid id);

        Task<T> AddAsync(T entity);

        Task AddRangeAsync(List<T> entities);

        Task UpdateAsync(T entity);

        Task UpdateRange(List<T> entities);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);

        Task DeleteByIdAsync(Guid id);
    }
}
