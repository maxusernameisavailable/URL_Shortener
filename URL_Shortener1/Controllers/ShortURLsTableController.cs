using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using URL_Shortener1.Models;
using URL_Shortener1.Services;
using URL_Shortener1.ViewModels;

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

        [HttpGet]
        public IActionResult ShortURLsTableView()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                var userUrls = _urlService.GetUserUrls(userId);
                return View(userUrls);
            }

            var urls = _urlService.GetUrls();
            return View(urls);
        }

        public IActionResult AboutView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ShortenUrl(UrlViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var url = await _urlService.ShortenUrlAsync(model.LongUrl, userId);
                return RedirectToAction("ShortURLsTableView", "ShortURLsTable");
            }

            return View("ShortURLsTableView");
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