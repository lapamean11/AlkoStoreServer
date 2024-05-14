namespace AlkoStoreServer.Models
{
    public class User
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Birthday { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public int RoleId { get; set; }

        public List<Review> Reviews { get; set; }

        public Role Role { get; set; }
    }
}
