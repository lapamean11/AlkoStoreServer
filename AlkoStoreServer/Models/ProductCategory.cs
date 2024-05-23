using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlkoStoreServer.Base;

namespace AlkoStoreServer.Models
{
    public class ProductCategory : Model
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public Product Product { get; set; }
    }
}
