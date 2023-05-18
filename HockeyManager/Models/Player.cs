using HockeyManager.Models;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;

namespace HockeyManager.Models
{
    public class Player
    {
        public int teamID { get; set; } = 0;
        public string teamname { get; set; } = "";
        public int number { get; set; } = 0;
        public string firstname { get; set; } = "";
        public string lastname { get; set; } = "";
        public string role { get; set; } = "";
        public int power { get; set; } = 0;
        public int price { get; set; } = 0;
        public int gamesplayed { get; set; } = 0;
        public int goals { get; set; } = 0;
        public int shots { get; set; } = 0;
        public int shotsagainst { get; set; } = 0;
        public int saves { get; set; } = 0;
        public int ID { get; set; } = 0;
    }
    public class TeamPlayers
    {
        public int TeamID { get; set; } = 0;
        public Player SelectedPlayer { get; set; }
        public List<Player> Players { get; set; }
        public string teamname { get; set; } = "";
    }

    public static class PlayerManager
    {
        public static List<Player> getAllUnownedPlayers()
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            List<Player> Players = new List<Player>();
            MySqlConnection conn = new MySqlConnection(conStr);
            MySqlCommand MyCom = new MySqlCommand("SELECT * FROM `Player` WHERE `TeamID` = 0", conn);

            conn.Open();

            MySqlDataReader reader = MyCom.ExecuteReader();

            while (reader.Read())
            {
                Player p = new Player();
                p.ID = reader.GetInt32("ID");
                p.teamID = reader.GetInt32("teamID");
                p.number = reader.GetInt32("number");
                p.firstname = reader.GetString("firstname");
                p.lastname = reader.GetString("lastname");
                p.role = reader.GetString("role");
                p.power = reader.GetInt32("power");
                p.price = reader.GetInt32("price");
                p.gamesplayed = reader.GetInt32("gamesplayed");
                p.goals = reader.GetInt32("goals");
                p.shots = reader.GetInt32("shots");
                p.shotsagainst = reader.GetInt32("shotsagainst");
                p.saves = reader.GetInt32("saves");
                Players.Add(p);
            }

            MyCom.Dispose();
            conn.Close();

            return Players;
        }

        public static TeamPlayers getAllOwnedPlayers(int teamID)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            string teamname = "";
            List<Player> Players = new List<Player>();

            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                conn.Open();

                MySqlCommand playerCmd = new MySqlCommand("SELECT * FROM `Player` LEFT JOIN Team ON Player.TeamID = Team.TeamID WHERE Team.TeamID = @teamID", conn);
                playerCmd.Parameters.AddWithValue("@teamID", teamID);

                MySqlDataReader reader = playerCmd.ExecuteReader();
                while (reader.Read())
                {
                    Player p = new Player();
                    p.ID = reader.GetInt32("ID");
                    p.teamID = reader.GetInt32("teamID");
                    p.number = reader.GetInt32("number");
                    p.firstname = reader.GetString("firstname");
                    p.lastname = reader.GetString("lastname");
                    p.role = reader.GetString("role");
                    p.power = reader.GetInt32("power");
                    p.price = reader.GetInt32("price");
                    p.gamesplayed = reader.GetInt32("gamesplayed");
                    p.goals = reader.GetInt32("goals");
                    p.shots = reader.GetInt32("shots");
                    p.shotsagainst = reader.GetInt32("shotsagainst");
                    p.saves = reader.GetInt32("saves");
                    Players.Add(p);
                }
                reader.Close();

                MySqlCommand teamCmd = new MySqlCommand("SELECT `Name`, `TeamID` FROM `Team` WHERE `TeamID` = @teamID", conn);
                teamCmd.Parameters.AddWithValue("@teamID", teamID);

                reader = teamCmd.ExecuteReader();
                if (reader.Read())
                {
                    teamname = reader.GetString("name");
                    teamID = reader.GetInt32("teamID");
                }
                reader.Close();

                playerCmd.Dispose();
                teamCmd.Dispose();
                conn.Close();
            }

            return new TeamPlayers { Players = Players, teamname = teamname, TeamID = teamID };
        }


        public static Player getSinglePlayerById(int id)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                conn.Open();

                using (MySqlCommand MyCom = new MySqlCommand("SELECT * FROM Player WHERE id = @ID", conn))
                {
                    MyCom.Parameters.AddWithValue("@ID", id);

                    using (MySqlDataReader reader = MyCom.ExecuteReader())
                    {
                        Player singleP = new Player();

                        if (reader.Read())
                        {
                            singleP.ID = reader.GetInt32("ID");
                            singleP.teamID = reader.GetInt32("teamID");
                            singleP.number = reader.GetInt32("number");
                            singleP.firstname = reader.GetString("firstname");
                            singleP.lastname = reader.GetString("lastname");
                            singleP.role = reader.GetString("role");
                            singleP.power = reader.GetInt32("power");
                            singleP.price = reader.GetInt32("price");
                            singleP.gamesplayed = reader.GetInt32("gamesplayed");
                            singleP.goals = reader.GetInt32("goals");
                            singleP.shots = reader.GetInt32("shots");
                            singleP.shotsagainst = reader.GetInt32("shotsagainst");
                            singleP.saves = reader.GetInt32("saves");
                        }

                        return singleP;
                    }
                }
            }
        }

        public static bool trainPlayer(Player p, int updatedPower, User u, int updatedCurrency)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";
            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                conn.Open();

                MySqlCommand playerCmd = new MySqlCommand("UPDATE `Player` SET `power` = @power WHERE `ID` = @ID", conn);
                playerCmd.Parameters.AddWithValue("@power", updatedPower);
                playerCmd.Parameters.AddWithValue("@ID", p.ID);

                int rader = playerCmd.ExecuteNonQuery();

                MySqlCommand userCmd = new MySqlCommand("UPDATE `User` SET `currency` = @currency WHERE `ID` = @ID", conn);
                userCmd.Parameters.AddWithValue("@currency", updatedCurrency);
                userCmd.Parameters.AddWithValue("@ID", u.ID);

                rader = userCmd.ExecuteNonQuery();

                playerCmd.Dispose();
                userCmd.Dispose();

                conn.Close();
                if (rader == 0)
                    return false;
                else
                    return true;
            }
        }



        public static bool savePlayer(Player p)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";
            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                conn.Open();

                MySqlCommand MyCom = new MySqlCommand("UPDATE `Player` SET `teamID` = @teamID, `number` = @number, `firstname` = @firstname, `lastname` = @lastname, `role` = @role, `power` = @power, `price` = @price, `gamesplayed` = @gamesplayed, `goals` = @goals, `shots` = @shots, `shotsagainst` = @shotsagainst, `saves` = @saves WHERE `ID` = @ID", conn);
                MyCom.Parameters.AddWithValue("@teamID", p.teamID);
                MyCom.Parameters.AddWithValue("@number", p.number);
                MyCom.Parameters.AddWithValue("@firstname", p.firstname);
                MyCom.Parameters.AddWithValue("@lastname", p.lastname);
                MyCom.Parameters.AddWithValue("@role", p.role);
                MyCom.Parameters.AddWithValue("@power", p.power);
                MyCom.Parameters.AddWithValue("@price", p.price);
                MyCom.Parameters.AddWithValue("@gamesplayed", p.gamesplayed);
                MyCom.Parameters.AddWithValue("@goals", p.goals);
                MyCom.Parameters.AddWithValue("@shots", p.shots);
                MyCom.Parameters.AddWithValue("@shotsagainst", p.shotsagainst);
                MyCom.Parameters.AddWithValue("@saves", p.saves);
                MyCom.Parameters.AddWithValue("@ID", p.ID);

                int rowsAffected = MyCom.ExecuteNonQuery();

                MyCom.Dispose();
                conn.Close();

                return rowsAffected > 0;
            }
        }

        public static bool savenewPlayer(Player p)
        {

            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            MySqlConnection conn = new MySqlConnection(conStr);
            MySqlCommand MyCom = new MySqlCommand("INSERT INTO Player(teamID, number, firstname, lastname, role, power, price, gamesplayed, goals, shots, shotsagainst, saves, ID) VALUES (@teamID, @number, @firstname, @lastname, @role, @power, @price, @gamesplayed, @goals, @shots, @shotsagainst, @saves, @ID)", conn);
            MyCom.Parameters.AddWithValue("@teamID", p.teamID);
            MyCom.Parameters.AddWithValue("@number", p.number);
            MyCom.Parameters.AddWithValue("@firstname", p.firstname);
            MyCom.Parameters.AddWithValue("@lastname", p.lastname);
            MyCom.Parameters.AddWithValue("@role", p.role);
            MyCom.Parameters.AddWithValue("@power", p.power);
            MyCom.Parameters.AddWithValue("@price", p.price);
            MyCom.Parameters.AddWithValue("@gamesplayed", p.gamesplayed);
            MyCom.Parameters.AddWithValue("@goals", p.goals);
            MyCom.Parameters.AddWithValue("@shots", p.shots);
            MyCom.Parameters.AddWithValue("@shotsagainst", p.shotsagainst);
            MyCom.Parameters.AddWithValue("@saves", p.saves);
            MyCom.Parameters.AddWithValue("@ID", p.ID);
            conn.Open();

            int rader = MyCom.ExecuteNonQuery();

            MyCom.Dispose();
            conn.Close();

            if (rader == 0) return false; else return true;


        }
        public static bool deletePlayer(Player p)
        {

            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            MySqlConnection conn = new MySqlConnection(conStr);
            MySqlCommand MyCom = new MySqlCommand("DELETE FROM Player WHERE id = @ID", conn);
            MyCom.Parameters.AddWithValue("@ID", p.ID);
            conn.Open();

            int rader = MyCom.ExecuteNonQuery();

            MyCom.Dispose();
            conn.Close();

            if (rader == 0) return false; else return true;


        }

    }

}