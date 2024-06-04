using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlkoStoreServer.Base;
using AlkoStoreServer.CustomAttributes;

namespace AlkoStoreServer.Models
{
    public class Role : Model
    {
        [NoRender]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Identifier { get; set; }

        public List<AdminUser> AdminUsers { get; set; }
    }
}
