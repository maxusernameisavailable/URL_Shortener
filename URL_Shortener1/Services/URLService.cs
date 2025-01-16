using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using URL_Shortener1.DBContext;
using URL_Shortener1.Models;

namespace URL_Shortener1.Services
{
    public interface IURLService
    {
        Task<URL> ShortenUrlAsync(string longUrl, string userId);
        IEnumerable<URL> GetUserUrls(string userId);
        IEnumerable<URL> GetUrls();
        Task DeleteAllAsync();
        string GetLongUrl(string shortUrl);
    }

    public class URLService : IURLService
    {
        private readonly ApplicationDBContext _dbContext;

        public URLService(ApplicationDBContext dbContext) 
        { 
            _dbContext = dbContext; 
        }

        public async Task DeleteAllAsync()
        {
            var urls = _dbContext.URLs.AsQueryable();

            _dbContext.URLs.RemoveRange(urls);

            await _dbContext.SaveChangesAsync();
        }

        public string GetLongUrl(string key)
        {
            var url = _dbContext.URLs.FirstOrDefault(u => u.ShortenedUrl.EndsWith($"{key}"));
            if (url != null)
            {
                return url.OriginalUrl;
            }
            return string.Empty;
        }

        public IEnumerable<URL> GetUrls()
        {
            return _dbContext.URLs.ToList();
        }

        public IEnumerable<URL> GetUserUrls(string userId)
        {
            return _dbContext.URLs.Where(user => user.UserId.ToString() == userId).ToList();
        }

        public async Task<URL> ShortenUrlAsync(string longUrl, string userId)
        {
            var check = _dbContext.URLs.FirstOrDefault(url => url.OriginalUrl == longUrl);
            if (check != null)
            {
                return null;
            }

            var shortenUrl = GenerateShortUrlAsync(longUrl);

            var url = new URL
            {
                OriginalUrl = longUrl,
                ShortenedUrl = shortenUrl,
                UserId = Int32.Parse(userId),
                CreatedDate = DateTime.UtcNow
            };

            await _dbContext.URLs.AddAsync(url);
            await _dbContext.SaveChangesAsync();

            return url;
        }

        private string GenerateShortUrlAsync(string longUrl) 
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashbytes = md5.ComputeHash(Encoding.UTF8.GetBytes(longUrl));

                byte[] shortenedHashbytes = new byte[8];
                Array.Copy(hashbytes, shortenedHashbytes, 6);

                long decimalVal = BitConverter.ToInt64(shortenedHashbytes, 0);
                string shortUrl = ToBase62(decimalVal);

                return $"https://localhost:7218/Shorten/{shortUrl}";
            }
        }

        private string ToBase62(long value)
        {
            const string base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder sb = new StringBuilder();

            while (value > 0)
            {
                sb.Insert(0, base62Chars[(int)(value % 62)]);
                value /= 62;
            }

            return sb.ToString();
        }
    }
}
