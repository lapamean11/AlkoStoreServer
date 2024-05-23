using AlkoStoreServer.Base;
using AlkoStoreServer.Data;
using AlkoStoreServer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AlkoStoreServer.Repositories
{
    public class AttributeRepository : BaseRepository, IAttributeRepository
    {
        public AttributeRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        public object getAttributes(string modelName, string entityId)
        {
            var dbSetProperty = _dbContext.GetType().GetProperties()
            .FirstOrDefault(p => p.PropertyType.GenericTypeArguments[0].Name == modelName);
            var type = ((IQueryable)_dbContext.GetType().GetProperty(dbSetProperty.Name).GetValue(_dbContext))
            .ElementType;

            return _dbContext.Find(type, int.TryParse(entityId, out int number) ? int.Parse(entityId) : entityId.ToString());
        }

        public List<Model> GetAllToList(Type modelType)
        {
            if (modelType != null && typeof(Model).IsAssignableFrom(modelType))
            {
                PropertyInfo dbSetProperty = _dbContext.GetType().GetProperties()
                    .SingleOrDefault(p => p.PropertyType.IsGenericType &&
                                          p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                                          p.PropertyType.GenericTypeArguments[0] == modelType);

                if (dbSetProperty != null)
                {
                    var dbSet = (IQueryable<Model>)dbSetProperty.GetValue(_dbContext);
                    var data = dbSet.ToList();

                    return data;
                }
            }

            return null;
        }
    }
}
