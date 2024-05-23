using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlkoStoreServer.Base;

namespace AlkoStoreServer.Models
{
    public class CategoryAttribute : Model
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public string Identifier { get; set; }

        public int TypeId { get; set; }

        public string Name { get; set; }

        public AttributeType AttributeType { get; set; }

        public List<CategoryAttributeCategory> Categories { get; set; }
    }
}
