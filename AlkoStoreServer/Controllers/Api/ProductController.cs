using AlkoStoreServer.Base;
using AlkoStoreServer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlkoStoreServer.Controllers.Api
{
    [Route("api")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("get/products")]
        public IActionResult GetProducts()
        {
            var products = _productRepository.GetProductsWithStores();

            return Ok(products);
        }

    }
}
