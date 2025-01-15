using System.Data.Entity.Core.Common.CommandTrees;
using System.Security.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using URL_Shortener1.Models;
using URL_Shortener1.ViewModels;

namespace URL_Shortener1.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User>? _signInManager;
        private readonly UserManager<User>? _userManager;
        private readonly RoleManager<Role>? _roleManager;

        public AccountController(SignInManager<User>? signInManager, UserManager<User>? userManager, RoleManager<Role>? roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Неправильний логін або пароль.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("ShortURLsTableView", "ShortURLsTable");
            }

            ModelState.AddModelError(string.Empty, "Неправильний логін або пароль.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("ShortURLsTableView", "ShortURLsTable");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User { UserName = model.Username };
            
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("ShortURLsTableView", "ShortURLsTable");
            }

            return View(model);
        }
    }
}
