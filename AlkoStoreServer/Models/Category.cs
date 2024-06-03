using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlkoStoreServer.Base;
using AlkoStoreServer.CustomAttributes;

namespace AlkoStoreServer.Models
{
    public class Category : Model
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public string? Name { get; set; }

        public string ImgUrl { get; set; }

        [NoRender]
        [Newtonsoft.Json.JsonIgnore]
        public int? ParentCategoryId { get; set; }

        [NoRender]
        public int? CategoryLevel { get; set; }

        [NoRender]
        public List<Category> ChildCategories { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [Reference(typeof(Category))]
        public Category? ParentCategory { get; set; }

        [NoRender]
        public List<ProductCategory> Products { get; set; }

        public List<CategoryAttributeCategory> CategoryAttributes { get; set; } = new List<CategoryAttributeCategory>();
    }
}
