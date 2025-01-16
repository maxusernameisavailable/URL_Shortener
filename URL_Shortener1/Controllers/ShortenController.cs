using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using URL_Shortener1.Services;

namespace URL_Shortener1.Controllers
{
    public class ShortenController : Controller
    {
        private readonly IURLService _urlService;

        public ShortenController(IURLService uRLService)
        {
            _urlService = uRLService;
        }

        [Route("Shorten/{key}")]
        [HttpGet]
        public IActionResult sh(string key)
        {
            var longUrl = _urlService.GetLongUrl(key);

            if (!string.IsNullOrEmpty(longUrl))
            {
                return Redirect(longUrl);
            }
            return NotFound();
        }
    }
}
