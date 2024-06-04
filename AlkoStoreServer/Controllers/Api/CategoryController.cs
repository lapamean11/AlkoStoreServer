using AlkoStoreServer.Base;
using AlkoStoreServer.Models;
using AlkoStoreServer.Models.Projections;
using AlkoStoreServer.Repositories;
using AlkoStoreServer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlkoStoreServer.Controllers.Api
{
    [Route("api")]
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly IDbRepository<Category> _categoryRepository;

        public CategoryController(
            IDbRepository<Category> categoryRepository
        )
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("get/categories")]
        public async Task<IActionResult> GetCategories()
        {
            List<Category> categories = (List<Category>) await _categoryRepository.GetWithInclude(
                 c => c.Include(e => e.ChildCategories).Where(ca => ca.ID != 1 && ca.CategoryLevel == 1)
            );

            var json = BaseRepository.SerializeToJson(categories);

            return Ok(json);
        }

        [HttpGet("get/category/{id}")]
        public async Task<IActionResult> GetCategoryWithProducts(int id)
        {
            CategoryProjection category = await _categoryRepository.GetById(id,
                u => new CategoryProjection
                {
                    ID = u.ID,
                    Name = u.Name,
                    CategoryLevel = u.CategoryLevel,
                    ImgUrl = u.ImgUrl,
                    Products = u.Products.Select(p => new ProductProjection 
                    {
                        ID = p.Product.ID,
                        Name = p.Product.Name,
                        ImgUrl = p.Product.ImgUrl,
                        IsPopular = p.Product.IsPopular,
                        Stores = p.Product.ProductStore.Select(sp => new StoreProjection 
                        {
                            Price = sp.Price
                        }).ToList(),
                    }).ToList(),
                    CategoryAttributes = u.CategoryAttributes.Select(ca => new AttributesProjection
                    {
                        ID = ca.Attribute.ID,
                        Name = ca.Attribute.Name,
                        Identifier = ca.Attribute.Identifier,
                        Value = ca.Value,
                        AttrType = ca.Attribute.AttributeType.Name
                    }).ToList(),
                    ChildCategories = u.ChildCategories.Select(cc => new CategoryProjection 
                    { 
                        ID= cc.ID,
                        Name = cc.Name,
                        CategoryLevel = cc.CategoryLevel
                    }).ToList(),
                },
                p => p.CategoryAttributes,
                p => p.Products
            );

            foreach (var product in category.Products)
            {
                decimal? lowestPrice = product.Stores.Any() ? product.Stores.Min(s => s.Price) : null;
                product.LowestPrice = lowestPrice;
                product.Stores = null;
            }

            var json = BaseRepository.SerializeToJson(category);

            return Ok(json);
        }
    }
}
