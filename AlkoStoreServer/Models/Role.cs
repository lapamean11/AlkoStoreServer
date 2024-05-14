namespace AlkoStoreServer.Models
{
    public class Role
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Identifier { get; set; }

        public List<User> Users { get; set; }
    }
}
