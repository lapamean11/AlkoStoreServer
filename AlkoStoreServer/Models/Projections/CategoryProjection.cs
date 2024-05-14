namespace AlkoStoreServer.Models.Projections
{
    public class CategoryProjection
    {
        public int ID { get; set; }

        public string? Name { get; set; }

        public int? CategoryId { get; set; }

        public int? CategoryLevel { get; set; }

        /*public List<ProductCategory> ProductCategory { get; set; }*/

        public List<AttributesProjection> CategoryAttributes { get; set; }
    }
}
