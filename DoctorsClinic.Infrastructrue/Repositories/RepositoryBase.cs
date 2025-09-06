using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DoctorsClinic.Infrastructure.Repositories
{
    public abstract class RepositoryBase<TModel, TID, TDbContext> : IRepositoryBase<TModel, TID>
        where TModel : BaseEntity<TID>, new()
        where TDbContext : DbContext
    {
        private bool _disposed;
        private TDbContext RepositoryContext { get; set; }

        protected RepositoryBase(TDbContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    RepositoryContext.Dispose();
                }
            }
            _disposed = true;
        }
       public IQueryable<TModel> FindByCondition(Expression<Func<TModel, bool>>? predicate = null,
            bool track = true, bool byTenant = true)
        {
            var query = RepositoryContext.Set<TModel>()
                .Where(w => !w.IsDeleted)
                .Where(predicate ?? throw new ArgumentNullException(nameof(predicate)));
            return track ? query.AsQueryable() : query.AsNoTracking().AsQueryable();
        }

        public async Task<TModel> FindItemByCondition(Expression<Func<TModel, bool>>? predicate = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>>? orderBy = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>>? include = null,
            bool track = true, bool byTenant = true)
        {
            var query = RepositoryContext.Set<TModel>()
                .Where(w => !w.IsDeleted)
                .Where(predicate ?? throw new ArgumentNullException(nameof(predicate)));

            if(include != null)
            {
                query = include(query);
            }
            if(orderBy != null)
            {
                query = orderBy(query);
            }
            if (track)
            {
                return (await query.FirstOrDefaultAsync())!;
            }
            return (await query.AsNoTracking().FirstOrDefaultAsync())!;
        }

        public void Add(TModel entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            RepositoryContext.Set<TModel>().Add(entity);
        }

        public void AddRange(List<TModel> entities)
        {
            entities.ForEach(e => e.CreatedAt = DateTime.UtcNow);
            RepositoryContext.Set<TModel>().AddRange(entities);
        }

        public async Task<bool> Insert(TModel entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await RepositoryContext.Set<TModel>().AddAsync(entity);
            return true;
        }

        public async Task<bool> InsertRange(List<TModel> entities)
        {
            entities.ForEach(e => e.CreatedAt = DateTime.UtcNow);
            await RepositoryContext.Set<TModel>().AddRangeAsync(entities);
            return true;
        }

        public async Task<TModel> Update(TModel entity)
        {
            entity.ModifieAt = DateTime.UtcNow;
            RepositoryContext.Entry(entity).State = EntityState.Modified;
            await RepositoryContext.SaveChangesAsync();
            return entity;
        }

        public bool UpdateRange(List<TModel> entities)
        {
            entities.ForEach(e => e.CreatedAt = DateTime.UtcNow);
            RepositoryContext.Set<TModel>().UpdateRange(entities);
            return true;
        }

        public async Task<TModel> Delete(TID id)
        {
            var entity = await RepositoryContext.Set<TModel>().FindAsync(id);
            if (entity == null)
            {
                return entity!;
            }
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            return entity;
        }
        
        public void DeleteRange(List<TModel> entities)
        {
            entities.ForEach(e => e.DeletedAt = DateTime.UtcNow);
            RepositoryContext.Set<TModel>().RemoveRange(entities);
        }

        public async Task<int> Commit()
        {
            return await RepositoryContext.SaveChangesAsync();
        }

        public Task<int> GetCountAsync()
        {
            IQueryable<TModel> query = RepositoryContext.Set<TModel>();
            return query.CountAsync();
        }

        public IQueryable<TModel> GetAll(bool byTenant = true)
        {
            IQueryable<TModel> query = RepositoryContext.Set<TModel>().Where(w => !w.IsDeleted);
            return query;
        }
    }
}