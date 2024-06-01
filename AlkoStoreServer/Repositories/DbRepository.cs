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

        public async Task<AppDbContext> GetContext() 
        {
            return _dbContext;
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

            T result = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "ID" ?? "UserId") == id);

            return result;
        }

        public async Task<T> GetById(int id, params Func<IQueryable<T>, IQueryable<T>>[] includeProperties)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            foreach (var includeProperty in includeProperties)
            {
                query = includeProperty(query);
            }

            T result = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "ID" ?? "UserId") == id);

            return result;
        }

        public async Task<TResult> GetById<TResult>(int id, Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            var result = await query.Where(e => EF.Property<int>(e, "ID" ?? "UserId") == id)
                                    .Select(selector)
                                    .FirstOrDefaultAsync();

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

        public async Task<int> CreateEntity(T entity)
        { 
            try
            {
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();

                var idProperty = entity.GetType().GetProperty("ID");

                if (idProperty == null)
                {
                    throw new InvalidOperationException("Entity does not have an ID property.");
                }

                return (int)idProperty.GetValue(entity);
            }
            catch (Exception ex) 
            {
                return 0;
            }
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
