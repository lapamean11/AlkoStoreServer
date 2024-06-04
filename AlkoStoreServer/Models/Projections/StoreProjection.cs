namespace AlkoStoreServer.Models.Projections
{
    public class StoreProjection
    {
        public int ID { get; set; }

        public string? Name { get; set; }

        public string? StoreLink { get; set; }

        public string? ImgUrl { get; set; }

        public decimal? Price { get; set; }

        public string? Barcode { get; set; }

        public int? Qty { get; set; }

        public List<ProductProjection>? Products { get; set; }
    }
}
