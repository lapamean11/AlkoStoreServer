using AlkoStoreServer.Models.Projections;

namespace AlkoStoreServer.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<CategoryProjection>> GetCategories();
    }
}
