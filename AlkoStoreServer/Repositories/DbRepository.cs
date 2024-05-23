using AlkoStoreServer.Base;
using AlkoStoreServer.Data;
using AlkoStoreServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AlkoStoreServer.Repositories
{
    public class DbRepository<T> : BaseRepository, IDbRepository<T> where T : class, new()
    {
        private DbSet<T> _dbSet; //readonly

        public DbRepository(
            AppDbContext dbContext
        ) : base(dbContext)
        {
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T> GetById(int id)
        {
            T result = await _dbSet.FindAsync(id);

            return result;
        }

        public async Task<T> GetById(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            T result = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "ID") == id);

            return result;
        }

        public async Task<T> GetById(int id, params Func<IQueryable<T>, IQueryable<T>>[] includeProperties)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            foreach (var includeProperty in includeProperties)
            {
                query = includeProperty(query);
            }

            T result = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "ID") == id);

            return result;
        }

        public async Task<IEnumerable<T>> GetWithInclude()
        {
            return await GetWithInclude(new Expression<Func<T, object>>[0]);
        }

        public async Task<IEnumerable<T>> GetWithInclude(params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<IEnumerable<T>> GetWithInclude(params Func<IQueryable<T>, IQueryable<T>>[] includeProperties)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            foreach (var includeProperty in includeProperties)
            {
                query = includeProperty(query);
            }

            var result = await query.ToListAsync();

            return result;
        }

        public async Task SaveEntity(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> CreateEntity(T entity)
        { 
            try
            {
                _dbContext.Add(entity);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }
    }
}
