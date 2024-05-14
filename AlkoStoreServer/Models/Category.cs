using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlkoStoreServer.Models
{
    public class Category
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public string? Name { get; set; }

        public int? CategoryId { get; set; }

        public int? CategoryLevel { get; set; }

        /*public List<ProductCategory> ProductCategory { get; set; }*/
        public List<Product>? Products { get; set; }

        public List<CategoryAttributeCategory> CategoryAttributes { get; set; }
    }
}
