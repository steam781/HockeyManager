using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static Google.Protobuf.WellKnownTypes.Field.Types;
using HockeyManager.Models;
using System.Collections.Generic;
using static HockeyManager.Models.Player;
using System.Numerics;
using Microsoft.AspNetCore.Identity;

namespace HockeyManager.Controllers
{
    public class GameController : Controller
    {
        string Message = "";
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
                TempData["Message"] = "team id was null >; look-> " + teamID;
            }

            Models.MyTeam.SaveTeamPositions(teamID.Value, t);

            MyTeam teamInfo = HockeyManager.Models.MyTeam.GetSingleTeamInfo(teamID.Value);

            if (teamInfo != null)
            {
                teamInfo.TeamPlayers = Models.PlayerManager.getAllOwnedPlayers(teamID.Value).Players.ToList();
            }

            TempData["Message"] = "Player positions saved " + t;

            return View("MyTeam", teamInfo);
        }



        public IActionResult MyTeamPlayer(int id)
        {
            Player p = PlayerManager.getSinglePlayerById(id);

            return View(p);
        }
        public IActionResult PlayerTrain(int id, Player p, int cost, int tempPower)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            int? teamId = HttpContext.Session.GetInt32("teamID");
            int? currency = HttpContext.Session.GetInt32("currency");

            int updatedCurrency;
            int updatedPower;
            int updatedPrice;

            User u = new User
            {
                ID = userId ?? 0,
                TeamID = teamId ?? 0,
                Currency = currency ?? 0
            };

            if (u.Currency >= cost)
            {
                if (p.power < 100 - tempPower)
                {
                    updatedCurrency = u.Currency - cost;
                    updatedPower = p.power + tempPower;
                    updatedPrice = p.price + (tempPower * 15);
                    PlayerManager.trainPlayer(p, updatedPower, u, updatedCurrency, updatedPrice);
                    HttpContext.Session.SetInt32("currency", updatedCurrency);
                }
                else
                {
                    TempData["Message"] = "Player already max power";
                }
            }
            else
            {
                TempData["Message"] = "You dont have enough money";
            }
            
            return View("MyTeamPlayer", p);
        }

        public IActionResult BuyPlayer(int id)
        {
            int userID = HttpContext.Session.GetInt32("id") ?? 0; 

            Player player = Models.PlayerManager.getSinglePlayerById(id);
            User user = Models.User.GetUserByID(userID);

            if (player != null && user != null)
            {
                int playerValue = player.price;

                if (user.Currency >= playerValue)
                {
                    PlayerManager.BuyPlayer(id, user.TeamID, player.price);
                    Models.User.DecreaseCurrency(userID, playerValue);
                    int oldCurrency = HttpContext.Session.GetInt32("currency") ?? 0;
                    if (oldCurrency != 0)
                    {
                        int newCurrency = oldCurrency - playerValue;
                        HttpContext.Session.SetInt32("currency", newCurrency);
                    }
                    TempData["Message"] = "Player succesfully purchased. Welcome " + player.firstname + " " + player.lastname + " to the team";
                    if (user.Currency >= 100000)
                    {
                        TempData["Message"] = "Upon showing the shere size of your bankaccount to " + player.firstname + " " + player.lastname + " they faint";
                    }
                }
                else
                {
                    // insufficient funds error
                    TempData["Message"] = user.Currency + "$ / 'YOU'RE BROKE! YOUR FUCKING POOR!' - Andre Tate 2022";
                }
            }
            else
            {
                //player or user not found error
                TempData["Message"] = "Player: " + player + " or User: " + user + " not found";
            }

            return RedirectToAction("Home", "Game");
        }



        public IActionResult SellPlayer(int id)
        {
            int userID = HttpContext.Session.GetInt32("id") ?? 0;

            Player player = Models.PlayerManager.getSinglePlayerById(id);
            User user = Models.User.GetUserByID(userID);

            if (player != null && user != null)
            {
                int playerValue = player.price;

                PlayerManager.SellPlayer(id, user.TeamID, player.price);
                Models.User.IncreaseCurrency(userID, playerValue);
                int oldCurrency = HttpContext.Session.GetInt32("currency") ?? 0;
                int newCurrency = oldCurrency - playerValue;
                HttpContext.Session.SetInt32("currency", newCurrency);
                TempData["Message"] = "Player succesfully sold. Welcome " + player.firstname + " " + player.lastname + " to the streets";
            }
            else
            {
                TempData["Message"] = "Player: " + player + " or User: " + user + " not found";
            }

            return RedirectToAction("Home", "Game");
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

        public IActionResult Market(int playerId)
        {
            TeamPlayers Players = Models.PlayerManager.getAllUnownedPlayers();
            ViewBag.SelectedPlayerId = playerId;

            return View("Player", Players);
        }

        public IActionResult Leaderboard()
        {
            TempData["Message"] = "";
            return View();
        }
        public IActionResult Statistics()
        {
            TempData["Message"] = "";
            return View();
        }
        public IActionResult HistoryT()
        {
            TempData["Message"] = "";
            int? teamID = HttpContext.Session.GetInt32("teamID");
            Trade trade = HockeyManager.Models.TradeManager.getAllTrades(teamID.Value);

            return View(trade);
        }
    }
}