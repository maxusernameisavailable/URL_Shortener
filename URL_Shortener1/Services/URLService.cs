using System.Data.Entity;
using URL_Shortener1.DBContext;
using URL_Shortener1.Models;

namespace URL_Shortener1.Services
{
    public interface IURLService
    {
        Task<URL> ShortenUrlAsync(string longUrl, string userId);
        IEnumerable<URL> GetUserUrls(string userId);
        IEnumerable<URL> GetUrls();
    }

    public class URLService : IURLService
    {
        private readonly ApplicationDBContext _dbContext;

        public URLService(ApplicationDBContext dbContext) 
        { 
            _dbContext = dbContext; 
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
                ShortenedUrl = shortenUrl.Result,
                UserId = Int32.Parse(userId),
                CreatedDate = DateTime.UtcNow
            };

            await _dbContext.URLs.AddAsync(url);
            await _dbContext.SaveChangesAsync();

            return url;
        }

        private async Task<string> GenerateShortUrlAsync(string longUrl) //logic
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
