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

        public AccountController(SignInManager<User>? signInManager, UserManager<User>? userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
    }
}
