using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static Google.Protobuf.WellKnownTypes.Field.Types;
using HockeyManager.Models;
using System.Collections.Generic;
using static HockeyManager.Models.Player;
using System.Numerics;

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


            MyTeam teamInfo = HockeyManager.Models.MyTeam.GetSingleTeamInfo(teamID.Value);

            if (teamInfo != null)
            {
                teamInfo.TeamPlayers = Models.PlayerManager.getAllOwnedPlayers(teamID.Value).Players.ToList();
                teamInfo.TeamPlayerRoles = teamInfo.TeamPlayers; // Set the TeamPlayerRoles list to the TeamPlayers list
            }

            return View(teamInfo);
        }



        public IActionResult MyTeamPartial(int id)
        {
            MyTeam teamInfo = HockeyManager.Models.MyTeam.GetSingleTeamInfo(id);
            teamInfo.TeamPlayerRoles = Models.PlayerManager.getAllOwnedPlayers(id).Players.ToList(); // Set the TeamPlayerRoles list
            ViewBag.SelectedPlayerId = id;
            return PartialView("MyTeamPartial", teamInfo);
        }



        public IActionResult SaveTeamPositions(MyTeam t)
        {
            int? teamID = HttpContext.Session.GetInt32("teamID");
            if (!teamID.HasValue)
            {
                // handle case where team ID is not set in session
            }

            // Call the SaveTeamPositions method and pass the team ID and MyTeam object
            Models.MyTeam.SaveTeamPositions(teamID.Value, t);

            MyTeam teamInfo = HockeyManager.Models.MyTeam.GetSingleTeamInfo(teamID.Value);

            if (teamInfo != null)
            {
                teamInfo.TeamPlayers = Models.PlayerManager.getAllOwnedPlayers(teamID.Value).Players.ToList();
            }

            return View("MyTeam", teamInfo);
        }



        public IActionResult MyTeamPlayer(int id)
        {

            Player p = PlayerManager.getSinglePlayerById(id);

            return View(p);
        }
        public IActionResult PlayerTrain(int id, Player p, int cost, int tempPower)
        {
            // Retrieve the user data from the session
            int? userId = HttpContext.Session.GetInt32("id");
            int? teamId = HttpContext.Session.GetInt32("teamID");
            int? currency = HttpContext.Session.GetInt32("currency");

            

            // Create a new User object with the retrieved data
            User u = new User
            {
                ID = userId ?? 0,
                TeamID = teamId ?? 0,
                Currency = currency ?? 0
            };


            // Calculate the updated currency and power values
            int updatedCurrency = u.Currency - cost;
            int updatedPower = p.power + tempPower;

            // Update the player's power and user's currency in the database
            PlayerManager.trainPlayer(p, updatedPower, u, updatedCurrency);

            // Update the Players Session
;
            HttpContext.Session.SetInt32("currency", updatedCurrency);

            return View("MyTeamPlayer", p);
        }


        public IActionResult Player(int playerId)
        {

            int? teamID = HttpContext.Session.GetInt32("teamID");
            if (!teamID.HasValue)
            {
                // handle case where team ID is not set in session
            }
            TeamPlayers teamPlayers = Models.PlayerManager.getAllOwnedPlayers(teamID.Value);
            playerId = 1;
            ViewBag.SelectedPlayerId = playerId;

            return View("Player", teamPlayers);
        }
        public IActionResult PlayerDetail(int id)
        {
            var player = PlayerManager.getSinglePlayerById(id);
            ViewBag.SelectedPlayerId = id;
            return PartialView("PlayerDetail", player);
        }

        public IActionResult Market()
        {
            List<Player> players = Models.PlayerManager.getAllUnownedPlayers();

            return View(players);
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
