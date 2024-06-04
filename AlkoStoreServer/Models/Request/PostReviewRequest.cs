namespace AlkoStoreServer.Models.Request
{
    public class PostReviewRequest
    {
        public int ProductId { get; set; }

        public int? Rating { get; set; }

        public string? Value { get; set; }
    }
}
