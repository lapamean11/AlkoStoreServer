namespace AlkoStoreServer.Models
{
    public class Review
    {
        public int ID { get; set; }

        public string Value { get; set; }

        public int Rrating { get; set; }

        public int ProductId { get; set; }

        public int UserId { get; set; }

        public string AddedAt { get; set; }

        public User User { get; set; }

        public Product Product { get; set; }
    }
}
