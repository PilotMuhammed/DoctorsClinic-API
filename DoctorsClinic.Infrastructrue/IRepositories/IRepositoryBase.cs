using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace DoctorsClinic.Infrastructure.IRepositories
{
    public interface IRepositoryBase<T, TKey> : IDisposable where T : class
    {
        IQueryable<T> GetAll(
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool track = false
        );

        Task<T?> GetByIdAsync(
            TKey id,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool track = false
        );

        IQueryable<T> FindByCondition(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool track = false
        );

        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        Task<int> SaveChangesAsync();
    }
}
