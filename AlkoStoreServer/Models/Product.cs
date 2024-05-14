using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlkoStoreServer.Models
{
    public class Product
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public List<Category> Categories { get; set; }

        public List<ProductStore> ProductStore { get; set; }

        public List<Review> Reviews { get; set; }

        public List<ProductAttributeProduct> ProductAttributes { get; set; }

    }
}
