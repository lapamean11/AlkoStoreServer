using AlkoStoreServer.Base;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlkoStoreServer.Services.Interfaces
{
    public interface IAttributeService
    {
        public object getAttributes(string modelName, string entityId);

        public List<Model> GetAllToList(Type modelName);

        public IDictionary<string, List<Model>> GetFormRelatedData(Model model);
    }
}
