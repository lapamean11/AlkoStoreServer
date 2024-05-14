namespace AlkoStoreServer.Models.Projections
{
    public class ProductProjection
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public List<StoreProjection>? Stores { get; set; }
    }
}
