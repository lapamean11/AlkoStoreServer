using AlkoStoreServer.Base;
using AlkoStoreServer.Models;
using AlkoStoreServer.Models.Projections;
using AlkoStoreServer.Repositories.Interfaces;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Cloud.Firestore;
using AlkoStoreServer.Models.Request;
using AlkoStoreServer.Services.Interfaces;

namespace AlkoStoreServer.Controllers.Api
{
    [Route("api")]
    [ApiController]
    public class ProductController : BaseController
    {

        private readonly IDbRepository<Product> _productRepository;

        private readonly IUserService _userService;

        private readonly FirestoreDb _firestoreDb;

        public ProductController(
            IDbRepository<Product> productRepository,
            IUserService userService
        ) {
            _productRepository = productRepository;
            _userService = userService;
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

            ProductProjection product = await _productRepository.GetById(id,
                   u => new ProductProjection 
                   {
                       ID = u.ID,
                       Name = u.Name,
                       ImgUrl = u.ImgUrl,
                       IsPopular = u.IsPopular,
                       Stores = u.ProductStore.Select(s => new StoreProjection
                       {
                           ID = s.Store.ID,
                           Name = s.Store.Name,
                           Price = s.Price,
                           Barcode = s.Barcode,
                           StoreLink = s.Store.StoreLink,
                           ImgUrl = s.Store.ImgUrl,
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
                               Email = s.User.Email
                           }
                       }).ToList()
                   },
                   p => p.ProductStore,
                   p => p.ProductAttributes,
                   p => p.Reviews
            );

            decimal? lowestPrice = product.Stores.Any() ? product.Stores.Min(s => s.Price) : null;
            product.LowestPrice = lowestPrice;

            foreach (var review in product.Reviews)
                review.User.Name = await _userService.GetUserNameByEmail(review.User.Email);

            var json = BaseRepository.SerializeToJson(product);

            return Ok(json);
        }

        [HttpGet("get/popular/products")]
        public async Task<IActionResult> GetPopularProducts()
        {
            List<ProductProjection> products = (List<ProductProjection>)await _productRepository.GetWithInclude(
                product => product.IsPopular == Product.IS_POPULAR,
                product => new ProductProjection
                {
                    ID = product.ID,
                    Name = product.Name,
                    ImgUrl = product.ImgUrl,
                    Stores = product.ProductStore.Select(sp => new StoreProjection
                    {
                        Price = sp.Price
                    }).ToList(),
                    ProductAttributes = product.ProductAttributes.Select(pa => new AttributesProjection 
                    {
                        ID = pa.Attribute.ID,
                        Name = pa.Attribute.Name,
                        Identifier = pa.Attribute.Identifier,
                        Value = pa.Value,
                        AttrType = pa.Attribute.AttributeType.Name
                    }).ToList()
                },
                product => product.ProductStore
            );

            foreach (var product in products)
            {
                product.LowestPrice = product.Stores.Any() ? product.Stores.Min(s => s.Price) : (decimal?)null;
            }

            var json = BaseRepository.SerializeToJson(products);

            return Ok(json);
        }

        [HttpGet("get/products/search/{key}")]
        public async Task<IActionResult> GetProductSearch(string key)
        {
            List<ProductProjection> products = (List<ProductProjection>) await _productRepository.GetWithInclude(
                product => (product.Name.ToLower().Replace(" ", "")).Contains((key.ToLower().Replace(" ", ""))),
                product => new ProductProjection
                { 
                    ID = product.ID,
                    Name = product.Name,
                    ImgUrl = product.ImgUrl,
                    IsPopular = product.IsPopular,
                    Stores = product.ProductStore.Select(sp => new StoreProjection
                    {
                        Price = sp.Price
                    }).ToList(),
                },
                product => product.ProductStore
            );

            foreach (var product in products)
            {
                product.LowestPrice = product.Stores.Any() ? product.Stores.Min(s => s.Price) : (decimal?)null;
            }

            var json = BaseRepository.SerializeToJson(products);

            return Ok(json);
        }
    }
}
