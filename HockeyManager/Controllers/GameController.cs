using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static Google.Protobuf.WellKnownTypes.Field.Types;
using HockeyManager.Models;
using System.Collections.Generic;

namespace HockeyManager.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult MyTeam(User usr, MyTeam team)
        {
            List<MyTeam> TeamInfo = Models.MyTeam.GetTeamInfo(usr.TeamID);

            return View(TeamInfo);
        }
        public IActionResult Player()
        {
            return View();
        }
        public IActionResult Market()
        {
            return View();
        }
        public IActionResult Leaderboard()
        {
            return View();
        }
        public IActionResult Statistics()
        {
            return View();
        }
        public IActionResult History()
        {
            return View();
        }
    }
}
