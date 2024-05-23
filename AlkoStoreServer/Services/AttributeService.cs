using AlkoStoreServer.Base;
using AlkoStoreServer.CustomAttributes;
using AlkoStoreServer.Repositories.Interfaces;
using AlkoStoreServer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace AlkoStoreServer.Services
{
    public class AttributeService : IAttributeService
    {
        private readonly IAttributeRepository _attributeRepository;


        public AttributeService(
            IAttributeRepository attributeRepository
        )
        {
            _attributeRepository = attributeRepository;
        }

        public object getAttributes(string modelName, string entityId)
        {
            if (CheckIfModelExists(modelName))
            {
                return _attributeRepository.getAttributes(modelName, entityId);
            }

            return null;
        }

        private bool CheckIfModelExists(string modelName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();

                if (types.Any(type => type.Name == modelName))
                {
                    return true;
                }
            }

            return false;
        }

        public List<Model> GetAllToList(Type modelName)
        {
            return _attributeRepository.GetAllToList(modelName);
        }

        public IDictionary<string, List<Model>> GetFormRelatedData(Model model)
        {
            IDictionary<string, List<Model>> relatedData = new Dictionary<string, List<Model>>();

            foreach (var prop in model.GetType().GetProperties())
            {
                if (Attribute.IsDefined(prop, typeof(ReferenceAttribute)))
                {
                    Type referenceModel = prop.GetCustomAttribute<ReferenceAttribute>().Reference;

                    var referenceData = GetAllToList(referenceModel);

                    relatedData.Add(prop.Name, referenceData);
                }
            }

            return relatedData;
        }
    }
}
