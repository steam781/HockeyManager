using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static Google.Protobuf.WellKnownTypes.Field.Types;
using HockeyManager.Models;
using System.Collections.Generic;
using static HockeyManager.Models.Player;

namespace HockeyManager.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult MyTeam()
        {
            int? teamID = HttpContext.Session.GetInt32("teamID");
            if (!teamID.HasValue)
            {
                // handle case where team ID is not set in session
            }

            TeamPlayers teamPlayers = Models.PlayerManager.getAllOwnedPlayers(teamID.Value);

            return View(teamPlayers);
        }
        public IActionResult Player()
        {

            return View(Player.getAllOwnedPlayers);
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
