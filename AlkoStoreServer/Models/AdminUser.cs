using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using AlkoStoreServer.Base;

namespace AlkoStoreServer.Models
{
    public class AdminUser : Model
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int RoleId { get; set; }

        public DateTime CreatedAt { get; set; }

        public Role Role { get; set; }

        public void SetPassword(string password)
        {
            Password = BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, Password);
        }
    }
}
