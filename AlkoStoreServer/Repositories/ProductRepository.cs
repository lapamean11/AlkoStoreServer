using AlkoStoreServer.Base;
using AlkoStoreServer.Data;
using AlkoStoreServer.Models.Projections;
using AlkoStoreServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlkoStoreServer.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(
            AppDbContext dbContext
        ) : base(dbContext)
        {

        }

        public async Task<IEnumerable<ProductProjection>> GetProductsWithStores()
        {
            IEnumerable<ProductProjection> products = await _dbContext.Product
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

                }).ToListAsync();

            return products;
        }
    }
}
