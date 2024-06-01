using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlkoStoreServer.Base;
using AlkoStoreServer.CustomAttributes;

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

        [NotMapped]
        public string DefaultValue { get; set; }

        [Reference(typeof(AttributeType))]
        public AttributeType AttributeType { get; set; }

        [NoRender]
        public List<CategoryAttributeCategory> Categories { get; set; }
    }
}
