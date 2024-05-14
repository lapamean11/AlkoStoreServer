using AlkoStoreServer.Base;
using AlkoStoreServer.Data;
using AlkoStoreServer.Models;
using AlkoStoreServer.Models.Projections;
using AlkoStoreServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace AlkoStoreServer.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(
            AppDbContext dbContext
        ) : base(dbContext)
        {

        }

        public IEnumerable<CategoryProjection> GetCategories()
        {
            
            IEnumerable<CategoryProjection> categories = _dbContext.Category
                    .Select(c => new CategoryProjection
                    {
                        ID = c.ID,
                        Name = c.Name,
                        CategoryId = c.CategoryId,
                        CategoryLevel = c.CategoryLevel,
                        CategoryAttributes = c.CategoryAttributes
                            .Select(cac => new AttributesProjection
                            {
                                ID = cac.ID,
                                Value = cac.Value,
                                Identifier = cac.Attribute.Identifier,
                                Name = cac.Attribute.Name,
                                AttributeType = cac.Attribute.AttributeType.Type
                            }).ToList()
                        }).ToList();

            return categories;

        }
    }
}
