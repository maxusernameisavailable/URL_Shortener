using System.ComponentModel.DataAnnotations;

namespace URL_Shortener1.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
