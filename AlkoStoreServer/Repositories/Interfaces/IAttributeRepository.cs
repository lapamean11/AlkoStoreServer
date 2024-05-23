using AlkoStoreServer.Base;

namespace AlkoStoreServer.Repositories.Interfaces
{
    public interface IAttributeRepository
    {
        public object getAttributes(string modelName, string entityId);

        public List<Model> GetAllToList(Type modelName);
    }
}
