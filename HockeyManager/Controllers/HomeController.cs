﻿using HockeyManager.Models;
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

            User nyBesökare = User.GetUserByMail(usr.Email);

            // Check if password is correct
            if (nyBesökare.Password != usr.password)
            {
                ViewBag.MeddelandePass = "Incorrect password";
                return View("Index");
            }

            HttpContext.Session.SetString("mailadress", newUser.mailadress);
            HttpContext.Session.SetString("name", newUser.TeamID);
            HttpContext.Session.SetString("role", newUser.Role);

            return RedirectToAction("Index", "Home");
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