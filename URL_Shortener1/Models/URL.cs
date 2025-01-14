namespace URL_Shortener1.Models
{
    public class URL
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; }

        public string ShortenedUrl { get; set; }
        public int CreatedBy { get; set; }
        public int CreatedDate { get; set; }
    }

}