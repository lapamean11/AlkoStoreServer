using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlkoStoreServer.Base;
using AlkoStoreServer.CustomAttributes;

namespace AlkoStoreServer.Models
{
    public class ProductAttributeProduct : Model
    {
        [NoRender]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int AttributeId { get; set; }

        public ProductAttribute Attribute { get; set; }

        public string Value { get; set; }
    }
}
