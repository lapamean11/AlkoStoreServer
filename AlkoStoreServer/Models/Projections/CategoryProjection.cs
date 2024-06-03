namespace AlkoStoreServer.Models.Projections
{
    public class CategoryProjection
    {
        public int ID { get; set; }

        public string? Name { get; set; }

        //public int? ParentCategoryId { get; set; }

        public string ImgUrl { get; set; }

        public int? CategoryLevel { get; set; }

        public List<AttributesProjection>? CategoryAttributes { get; set; }

        public List<ProductProjection>? Products { get; set; }

        public List<CategoryProjection>? ChildCategories { get; set; }
    }
}
