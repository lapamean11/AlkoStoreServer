using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlkoStoreServer.Base;
using AlkoStoreServer.CustomAttributes;

namespace AlkoStoreServer.Models
{
    public class Product : Model
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        [Reference(typeof(Category))]
        public List<Category> Categories { get; set; }

        [Reference(typeof(Store))]
        public List<ProductStore> ProductStore { get; set; }

        public List<Review> Reviews { get; set; }

        public List<ProductAttributeProduct> ProductAttributes { get; set; }

    }
}
