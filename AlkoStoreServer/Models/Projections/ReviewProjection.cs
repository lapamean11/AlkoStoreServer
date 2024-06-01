namespace AlkoStoreServer.Models.Projections
{
    public class ReviewProjection
    {
        public int ID { get; set; }

        public string? Value { get; set; }

        public int Rating { get; set; }

        public DateTime AddetAt { get; set; }

        public UserProjection? User {  get; set; } 
    }
}
