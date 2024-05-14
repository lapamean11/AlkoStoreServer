using AlkoStoreServer.Models.Projections;

namespace AlkoStoreServer.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        public IEnumerable<CategoryProjection> GetCategories();
    }
}
