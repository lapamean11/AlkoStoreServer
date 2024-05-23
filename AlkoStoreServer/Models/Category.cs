using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlkoStoreServer.Base;

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

        // [NotMapped]
        public List<Category> ChildCategories { get; set; }

        public Category ParentCategory { get; set; }

        public List<Product>? Products { get; set; }

        public List<CategoryAttributeCategory> CategoryAttributes { get; set; }
    }
}
