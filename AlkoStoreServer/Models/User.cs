using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlkoStoreServer.Base;

namespace AlkoStoreServer.Models
{
    public class User : Model
    {
        [Key]
        [Required]
        public string Email { get; set; }

        public List<Review> Reviews { get; set; }
    }
}
