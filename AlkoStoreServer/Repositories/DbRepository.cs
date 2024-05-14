using AlkoStoreServer.Base;
using AlkoStoreServer.Data;
using AlkoStoreServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AlkoStoreServer.Repositories
{
    public class DbRepository<T> : BaseRepository, IDbRepository<T> where T : class, new()
    {
        private readonly DbSet<T> _dbSet;

        public DbRepository(
            AppDbContext dbContext
        ) : base(dbContext)
        {
            _dbSet = _dbContext.Set<T>();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public T GetById(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            T result = query.FirstOrDefault(e => _dbContext.Entry(e).Property<int>("Id").CurrentValue == id);

            return result;
        }

        public T GetById(int id, params Func<IQueryable<T>, IQueryable<T>>[] includeProperties)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            foreach (var includeProperty in includeProperties)
            {
                query = includeProperty(query);
            }

            T result = query.FirstOrDefault(e => _dbContext.Entry(e).Property<int>("Id").CurrentValue == id);

            return result;
        }

        public IEnumerable<T> GetWithInclude() => GetWithInclude(new Expression<Func<T, object>>[0]);

        public IEnumerable<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }

        public IEnumerable<T> GetWithInclude(params Func<IQueryable<T>, IQueryable<T>>[] includeProperties)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            foreach (var includeProperty in includeProperties)
            {
                query = includeProperty(query);
            }

            return query.ToList();
        }

        public void SaveEntity(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
