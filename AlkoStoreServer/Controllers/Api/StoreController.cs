using AlkoStoreServer.Base;
using AlkoStoreServer.Models;
using AlkoStoreServer.Models.Projections;
using AlkoStoreServer.Repositories;
using AlkoStoreServer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlkoStoreServer.Controllers.Api
{
    [Route("api")]
    [ApiController]
    public class StoreController : BaseController
    {
        private readonly IDbRepository<Store> _storeRepository;

        public StoreController(
            IDbRepository<Store> storeRepository
        )
        {
            _storeRepository = storeRepository;
        }

        [HttpGet("get/stores")]
        public async Task<IActionResult> GetCategories()
        {
            List<Store> stores = (List<Store>)await _storeRepository.GetWithInclude();

            var json = BaseRepository.SerializeToJson(stores);

            return Ok(json);
        }

        [HttpGet("get/store/{id}")]
        public async Task<IActionResult> GetStoreWithProducts(int id)
        { 
            StoreProjection store = await _storeRepository.GetById(id,
                u => new StoreProjection
                { 
                    ID = u.ID,
                    Name = u.Name,
                    StoreLink = u.StoreLink,
                    ImgUrl = u.ImgUrl,
                    Products = u.ProductStore.Select(p => new ProductProjection
                    {
                        ID = p.Product.ID,
                        Name = p.Product.Name,
                        ImgUrl = p.Product.ImgUrl,
                        IsPopular = p.Product.IsPopular,
                        Stores = p.Product.ProductStore.Select(sp => new StoreProjection
                        {
                            Price = sp.Price
                        }).ToList(),
                        ProductAttributes = p.Product.ProductAttributes.Select(pa => new AttributesProjection
                        {
                            ID = pa.Attribute.ID,
                            Name = pa.Attribute.Name,
                            Identifier = pa.Attribute.Identifier,
                            Value = pa.Value,
                            AttrType = pa.Attribute.AttributeType.Name
                        }).ToList(),
                    }).ToList(),
                },
                p => p.ProductStore
            );

            foreach (var product in store.Products)
            {
                // Calculate the lowest price for each product on the client side
                decimal? lowestPrice = product.Stores.Any() ? product.Stores.Min(s => s.Price) : null;
                product.LowestPrice = lowestPrice;
                product.Stores = null;
            }

            var json = BaseRepository.SerializeToJson(store);

            return Ok(json);
        }
    }
}
