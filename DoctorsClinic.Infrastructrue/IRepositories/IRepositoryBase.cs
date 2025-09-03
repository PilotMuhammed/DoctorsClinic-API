using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace DoctorsClinic.Infrastructure.IRepositories
{
    public interface IRepositoryBase<T, TID> : IDisposable
    {
        IQueryable<T> FindByCondition(Expression<Func<T, bool>>? predicate = null,
            bool track = true, bool byTenant = true);
        Task<T> FindItemByCondition(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool track = true, bool byTenant = true);

        void Add(T entity);
        void AddRange(List<T> entities);
        Task<bool> Insert(T entity);
        Task<bool> InsertRange(List<T> entities);
        Task<T> Update(T entity);
        bool UpdateRange(List<T> entities);
        Task<T> Delete(TID id);
        void DeleteRange(List<T> entities);
        Task<int> Commit();
        Task<int> GetCountAsync();
        IQueryable<T> GetAll(bool byTenant = true);
    }
}