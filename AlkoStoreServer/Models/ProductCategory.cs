using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlkoStoreServer.Base;
using AlkoStoreServer.CustomAttributes;

namespace AlkoStoreServer.Models
{
    public class ProductCategory : Model
    {
        [NoRender]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public Product Product { get; set; }
    }
}
