using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using AlkoStoreServer.Base;
using AlkoStoreServer.CustomAttributes;

namespace AlkoStoreServer.Models
{
    public class AttributeType : Model
    {
        [NoRender]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public List<ProductAttribute> ProductAttribute { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public List<CategoryAttribute> CategoryAttribute { get; set; }
    }
}
