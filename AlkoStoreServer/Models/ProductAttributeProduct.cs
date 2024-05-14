using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlkoStoreServer.Models
{
    public class ProductAttributeProduct
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int AttributeId { get; set; }

        public ProductAttribute Attribute { get; set; }

        public string Value { get; set; }
    }
}
