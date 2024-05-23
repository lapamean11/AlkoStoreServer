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

        public Task SaveEntity(T entity);

        public Task<bool> CreateEntity(T entity);
    }
}
