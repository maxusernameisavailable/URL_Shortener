using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace URL_Shortener1.Models
{
    public class User : IdentityUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
        public string? Role { get; set; }

        public ICollection<URL>? URLs { get; set; }
    }
}
