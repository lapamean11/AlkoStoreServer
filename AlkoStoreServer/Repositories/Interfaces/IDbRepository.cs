using System.Linq.Expressions;

namespace AlkoStoreServer.Repositories.Interfaces
{
    public interface IDbRepository<T> where T : class
    {
        public IEnumerable<T> GetWithInclude();

        public IEnumerable<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties);

        public IEnumerable<T> GetWithInclude(params Func<IQueryable<T>, IQueryable<T>>[] includeProperties);

        public T GetById(int id);

        public T GetById(int id, params Expression<Func<T, object>>[] includeProperties);

        public T GetById(int id, params Func<IQueryable<T>, IQueryable<T>>[] includeProperties);

        public void SaveEntity(T entity);
    }
}
