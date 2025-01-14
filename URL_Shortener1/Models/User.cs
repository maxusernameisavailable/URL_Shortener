using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace URL_Shortener1.Models
{
    public class User : IdentityUser
    { 
        public ICollection<URL>? URLs { get; set; }
    }
}
