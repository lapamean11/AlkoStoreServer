using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlkoStoreServer.Base;

namespace AlkoStoreServer.Models
{
    public class User : Model
    {
        [Key]
        [Required]
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public DateTime Birthday { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public int RoleId { get; set; }

        public List<Review> Reviews { get; set; }

        public Role Role { get; set; }
    }
}
