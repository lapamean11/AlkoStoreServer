using AlkoStoreServer.Models.Projections;

namespace AlkoStoreServer.Repositories.Interfaces
{
    public interface IProductRepository
    {
        public IEnumerable<ProductProjection> GetProductsWithStores();

    }
}
