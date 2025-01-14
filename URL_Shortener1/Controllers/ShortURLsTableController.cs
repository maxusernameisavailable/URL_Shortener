using Microsoft.AspNetCore.Mvc;

namespace URL_Shortener1.Controllers
{
    public class ShortURLsTableController : Controller
    {
        public IActionResult ShortURLsTableView()
        {
            return View();
        }

        public IActionResult AboutView()
        {
            return View();
        }
    }
}