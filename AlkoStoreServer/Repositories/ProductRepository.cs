using AlkoStoreServer.Base;
using AlkoStoreServer.Data;
using AlkoStoreServer.Models;
using AlkoStoreServer.Models.Projections;
using AlkoStoreServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlkoStoreServer.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        private readonly IDbRepository<Product> _productRepository;

        public ProductRepository(
            AppDbContext dbContext,
            IDbRepository<Product> productRepository
        ) : base(dbContext)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductProjection>> GetProductsWithStores()
        {
            /*IEnumerable<ProductProjection> products = await _dbContext.Product
                .Include(p => p.ProductStore)
                .ThenInclude(ps => ps.Store)
                .Select(p => new ProductProjection {
                    ID = p.ID,
                    Name = p.Name,
                    Stores = p.ProductStore.Select(ps => new StoreProjection 
                    {
                        ID = ps.Store.ID,
                        Name = ps.Store.Name,
                        Country = ps.Store.Country,
                        Price = ps.Price,
                        Barcode = ps.Barcode,
                        Products = null
                    }).ToList()

                }).ToListAsync();*/

            return null;
        }

        public async Task<ProductProjection> GetProductById(int id)
        {
            return null;
            /*ProductProjection product = await _dbContext.Product
                       .Include(e => e.Reviews)
                        .ThenInclude(e => e.User)
                       .Include(e => e.ProductAttributes)
                         .ThenInclude(e => e.Attribute)
                       .Include(e => e.ProductStore)
                         .ThenInclude(e => e.Store)
                       .Select(u => new ProductProjection
                       {
                           ID = u.ID,
                           Name = u.Name,
                           //Categories = null,
                           Stores = u.ProductStore.Select(s => new StoreProjection
                           {
                               ID = s.Store.ID,
                               Name = s.Store.Name,
                               Country = s.Store.Country,
                               Price = s.Price,
                               Barcode = s.Barcode,
                               Qty = s.Qty,
                               //Products = null
                           }).ToList(),
                           ProductAttributes = u.ProductAttributes.Select(pa => new AttributesProjection
                           {
                               ID = pa.Attribute.ID,
                               Name = pa.Attribute.Name,
                               Identifier = pa.Attribute.Identifier,
                               Value = pa.Value,
                               AttrType = pa.Attribute.AttributeType.Type
                           }).ToList(),
                           Reviews = u.Reviews.Select(s => new ReviewProjection
                           {
                               ID = s.ID,
                               Value = s.Value,
                               Rating = s.Rating,
                               AddetAt = DateTime.Now,
                               User = new UserProjection
                               {
                                   Name = s.User.Name,
                                   Username = s.User.Username
                               }
                           }).ToList()

                       }).FirstOrDefaultAsync(p => p.ID == id);

            return product;*/
        }
    }
}
