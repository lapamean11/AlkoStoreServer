using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlkoStoreServer.Base;
using AlkoStoreServer.CustomAttributes;

namespace AlkoStoreServer.Models
{
    public class Review : Model
    {
        [NoRender]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public string Value { get; set; }

        public int Rating { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int ProductId { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string UserId { get; set; }

        public DateTime AddedAt { get; set; }

        public User User { get; set; }

        public Product Product { get; set; }
    }
}
