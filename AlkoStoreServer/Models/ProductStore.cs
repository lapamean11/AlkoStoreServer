using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlkoStoreServer.Base;

namespace AlkoStoreServer.Models
{
    public class ProductStore : Model
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int ProductId { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int StoreId { get; set; }

        public decimal Price { get; set; }

        public string Barcode { get; set; }

        public int Qty { get; set; }

        public Product Product { get; set; }

        public Store Store { get; set; }
    }
}
