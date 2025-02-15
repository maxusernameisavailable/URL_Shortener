﻿using Microsoft.AspNetCore.Authorization;
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
                ViewBag.UserId = userId;
                var concat = _urlService.GetUserUrls(userId).Concat(_urlService.GetUrls()).Distinct();
                return View(concat);
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
            var userId = _userManager.GetUserId(User);
            ViewBag.UserId = userId;
            var url = await _urlService.ShortenUrlAsync(model.LongUrl, userId);
            if (url is null)
            {
                ModelState.AddModelError(string.Empty, "The provided URL already exists");
            }
            var concat = _urlService.GetUserUrls(userId).Concat(_urlService.GetUrls()).Distinct();
            return View("ShortURLsTableView", concat);
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAllRecords()
        {
            await _urlService.DeleteAllAsync();
            return View("ShortURLsTableView", _urlService.GetUrls());
        }
        
    }
}