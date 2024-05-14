using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AlkoStoreServer.Models
{
    public class AttributeType
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Type { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public List<ProductAttribute> ProductAttribute { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public List<CategoryAttribute> CategoryAttribute { get; set; }


    }
}
