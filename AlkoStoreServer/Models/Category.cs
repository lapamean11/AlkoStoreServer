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

        public int? ParentCategoryId { get; set; }

        public int? CategoryLevel { get; set; }

        [NoRender]
        public List<Category> ChildCategories { get; set; }

        [Reference(typeof(Category))]
        public Category? ParentCategory { get; set; }

        /*[NoRender]
        public List<Product>? Products { get; set; }*/

        [NoRender]
        public List<ProductCategory> Products { get; set; }

        public List<CategoryAttributeCategory> CategoryAttributes { get; set; } = new List<CategoryAttributeCategory>();
    }
}
