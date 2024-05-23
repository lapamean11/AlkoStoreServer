using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlkoStoreServer.Base;

namespace AlkoStoreServer.Models
{
    public class Review : Model
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public string Value { get; set; }

        public int Rrating { get; set; }

        public int ProductId { get; set; }

        public string UserId { get; set; }

        public string AddedAt { get; set; }

        public User User { get; set; }

        public Product Product { get; set; }
    }
}
