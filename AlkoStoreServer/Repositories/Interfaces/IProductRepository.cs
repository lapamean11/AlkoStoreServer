using AlkoStoreServer.Models.Projections;

namespace AlkoStoreServer.Repositories.Interfaces
{
    public interface IProductRepository
    {
        public Task<IEnumerable<ProductProjection>> GetProductsWithStores();

        public Task<ProductProjection> GetProductById(int id);

    }
}
