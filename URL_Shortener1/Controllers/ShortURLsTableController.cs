using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using URL_Shortener1.DBContext;
using URL_Shortener1.Models;
using URL_Shortener1.Services;
using URL_Shortener1.ViewModels;

namespace URL_Shortener1.Controllers
{
    public class ShortURLsTableController : Controller
    {
        private readonly IURLService _urlService;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDBContext _dbContext;

        public ShortURLsTableController(IURLService uRLService, UserManager<User> userManager, ApplicationDBContext dBContext)
        {
            _urlService = uRLService;
            _userManager = userManager;
            _dbContext = dBContext;
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
        [Authorize]
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

        [HttpGet]
        [Authorize]
        public IActionResult ShortURLInfoView(int id)
        {
            var url = _dbContext.URLs.Where(url => url.Id == id).FirstOrDefault();
            if (url != null) return View(url);
            return View();
        }

        [HttpGet]
        public IActionResult DeleteURLInfo(int id)
        {
            var url = _dbContext.URLs.Where(url => url.Id == id).FirstOrDefault();
            if (url != null)
            {
                _dbContext.URLs.Remove(url);
                _dbContext.SaveChangesAsync();
                return RedirectToAction("ShortURLsTableView", "ShortURLsTable");
            }
            return View();
        }
    }
}