using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace URL_Shortener1.Models
{
    public class User : IdentityUser<int>
    {
/*        public User()
        {
            this.SecurityStamp = Guid.NewGuid().ToString();
        }
*/
        public ICollection<URL>? URLs { get; set; }
    }
}
