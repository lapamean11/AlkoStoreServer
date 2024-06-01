using AlkoStoreServer.Data;
using System.Linq.Expressions;

namespace AlkoStoreServer.Repositories.Interfaces
{
    public interface IDbRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetWithInclude();

        public Task<IEnumerable<T>> GetWithInclude(params Expression<Func<T, object>>[] includeProperties);

        public Task<IEnumerable<T>> GetWithInclude(params Func<IQueryable<T>, IQueryable<T>>[] includeProperties);

        public Task<T> GetById(int id);

        public Task<T> GetById(int id, params Expression<Func<T, object>>[] includeProperties);

        public Task<T> GetById(int id, params Func<IQueryable<T>, IQueryable<T>>[] includeProperties);

        public Task<TResult> GetById<TResult>(int id, Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includeProperties);

        public Task SaveEntity(T entity);

        public Task<int> CreateEntity(T entity);

        public Task Add(T entity);

        public Task AddRange(IEnumerable<T> entities);

        public Task<AppDbContext> GetContext();

        public Task DeleteAsync(int id);
    }
}
