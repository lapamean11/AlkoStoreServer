using AlkoStoreServer.Base;
using AlkoStoreServer.Models;
using AlkoStoreServer.Models.Projections;
using AlkoStoreServer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AlkoStoreServer.Controllers.Api
{
    [Route("api")]
    [ApiController]
    public class ProductController : BaseController
    {

        private readonly IDbRepository<Product> _productRepository;

        public ProductController(
            IDbRepository<Product> productRepository
        ) {
            _productRepository = productRepository;
        }

        [HttpGet("get/products")]
        public async Task<IActionResult> GetProducts()
        {
            //var products = await _productRepository.GetProductsWithStores();

            var products2 = await _productRepository.GetWithInclude(
                    p => p.Include(e => e.Categories)
            );

            IEnumerable<Product> products3 = await _productRepository.GetWithInclude(
                p => p.Include(e => e.Categories)
                        .Include(e => e.ProductStore)
                            .ThenInclude(e => e.Store)
                          .Include(e => e.ProductAttributes)
                            .ThenInclude(e => e.Attribute)
                              .ThenInclude(e => e.AttributeType)
            );


            var json2 = BaseRepository.SerializeToJson(products2);

            return Ok(products3);
        }


        [HttpGet("get/product/{id}")]
        public async Task<IActionResult> GetProductWithReviews(int id)
        {
            ProductProjection product3 = await _productRepository.GetById(id,
                   u => new ProductProjection 
                   {
                       ID = u.ID,
                       Name = u.Name,
                       Stores = u.ProductStore.Select(s => new StoreProjection
                       {
                           ID = s.Store.ID,
                           Name = s.Store.Name,
                           Price = s.Price,
                           Barcode = s.Barcode,
                           Qty = s.Qty,
                       }).ToList(),
                       ProductAttributes = u.ProductAttributes.Select(pa => new AttributesProjection
                       {
                           ID = pa.Attribute.ID,
                           Name = pa.Attribute.Name,
                           Identifier = pa.Attribute.Identifier,
                           Value = pa.Value,
                           AttrType = pa.Attribute.AttributeType.Name
                       }).ToList(),
                       Reviews = u.Reviews.Select(s => new ReviewProjection
                       {
                           ID = s.ID,
                           Value = s.Value,
                           Rating = s.Rating,
                           AddetAt = DateTime.Now, /////////////////
                           User = new UserProjection
                           {
                               Name = s.User.Name,
                               Username = s.User.Username
                           }
                       }).ToList()
                   },
                   p => p.ProductStore,
                   p => p.ProductAttributes,
                   p => p.Reviews
            );

            var json = BaseRepository.SerializeToJson(product3);

            return Ok(json);
        }
    }
}
