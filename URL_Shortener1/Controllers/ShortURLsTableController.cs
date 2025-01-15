using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using URL_Shortener1.Models;
using URL_Shortener1.Services;

namespace URL_Shortener1.Controllers
{
    public class ShortURLsTableController : Controller
    {
        private readonly IURLService _urlService;
        private readonly UserManager<User> _userManager;

        public ShortURLsTableController(IURLService uRLService, UserManager<User> userManager)
        {
            _urlService = uRLService;
            _userManager = userManager;
        }

        public IActionResult ShortURLsTableView()
        {
            return View();
        }

        public IActionResult AboutView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ShortenUrl(string longUrl)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(longUrl))
            {
                BadRequest();
            }

            var url = await _urlService.ShortenUrlAsync(longUrl, userId);
            return Json(url);
        }

/*        [HttpPost]
        public async Task<IActionResult> GetUrls()
        {
            var userId = _userManager.GetUserId(User);
            var urls = await _urlService.GetUserUrlsAsync(userId);
            return Json(urls);
        }*/
    }
}