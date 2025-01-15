using System.Data.Entity;
using System.Reflection.Metadata.Ecma335;
using URL_Shortener1.DBContext;
using URL_Shortener1.Models;

namespace URL_Shortener1.Services
{
    public interface IURLService
    {
        Task<URL> ShortenUrlAsync(string longUrl, string userId);
        Task<IEnumerable<URL>> GetUserUrlsAsync(string userId);
    }

    public class URLService : IURLService
    {
        private readonly ApplicationDBContext _dbContext;

        public URLService(ApplicationDBContext dbContext) 
        { 
            _dbContext = dbContext; 
        }

        public async Task<IEnumerable<URL>> GetUserUrlsAsync(string userId)
        {
            return await _dbContext.URLs.Where(user => user.UserId.ToString() == userId).ToListAsync();
        }

        public async Task<URL> ShortenUrlAsync(string longUrl, string userId)
        {
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
