using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace URL_Shortener1.Models
{
    public class URL
    {
        [Key]
        public int Id { get; set; }
        public string? OriginalUrl { get; set; }

        public string? ShortenedUrl { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }         // remake migrations
        public User? User { get; set; }
    }

}