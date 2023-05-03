using HockeyManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HockeyManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User usr)
        {
            
            ModelState.Remove("TeamID");
            ModelState.Remove("Role");

            if (!ModelState.IsValid) return View("index");

            User newUser = HockeyManager.Models.User.GetUserByMail(usr.Email);
           

            // Check if password is correct
            if (newUser.Password != usr.Password)
            {
                ViewBag.MeddelandePass = "Incorrect password";
                return View("Index");
            }

            HttpContext.Session.SetInt32("id", newUser.ID);
            HttpContext.Session.SetInt32("teamID", newUser.TeamID);
            HttpContext.Session.SetString("role", newUser.Role);

            return RedirectToAction("Home", "Game");
        }

        public IActionResult Rules()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}