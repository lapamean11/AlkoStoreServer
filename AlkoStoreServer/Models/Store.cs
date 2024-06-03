using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlkoStoreServer.Base;
using AlkoStoreServer.CustomAttributes;

namespace AlkoStoreServer.Models
{
    public class Store : Model
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string StoreLink { get; set; }

        [NoRender]
        [Reference(typeof(Product))]
        public List<ProductStore> ProductStore { get; set; }
    }
}
