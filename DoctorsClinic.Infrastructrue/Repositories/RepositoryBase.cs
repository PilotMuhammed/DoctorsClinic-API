using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public abstract class RepositoryBase<T, TKey> : IRepositoryBase<T, TID>
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public virtual IQueryable<T> GetAll(
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool track = false)
        {
            IQueryable<T> query = _dbSet;
            if (!track)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return query;
        }

        public virtual IQueryable<T> FindByCondition(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool track = false)
        {
            IQueryable<T> query = _dbSet.Where(predicate);

            if (!track)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return query;
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate != null)
                return await _dbSet.CountAsync(predicate);
            return await _dbSet.CountAsync();
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public abstract Task<T?> GetByIdAsync(
            TKey id,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool track = false);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
