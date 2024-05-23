using AlkoStoreServer.Base;
using AlkoStoreServer.Models;
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
        private readonly IProductRepository _productRepository;

        private readonly IDbRepository<Product> _dbProductRepo;

        public ProductController(
            IProductRepository productRepository,
            IDbRepository<Product> dbProductRepo
        ) {
            _productRepository = productRepository;
            _dbProductRepo = dbProductRepo;
        }

        [HttpGet("get/products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepository.GetProductsWithStores();

            var products2 = await _dbProductRepo.GetWithInclude(
                    p => p.Include(e => e.Categories)
            );

            IEnumerable<Product> products3 = await _dbProductRepo.GetWithInclude(
                p => p.Include(e => e.Categories)
                        .Include(e => e.ProductStore)
                            .ThenInclude(e => e.Store)
                          .Include(e => e.ProductAttributes)
                            .ThenInclude(e => e.Attribute)
                              .ThenInclude(e => e.AttributeType)
            );


            var json2 = BaseRepository.SerializeToJson(products);

            return Ok(products3);
        }

    }
}
