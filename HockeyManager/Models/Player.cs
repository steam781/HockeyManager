using Webbshop.Models;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;

namespace Webbshop.Models
{
    public class Player
    {
        public int teamID { get; set; } = 0;
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

        public static List<Player> getAllUnownedPlayers()
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            List<Player> list = new List<Player>();
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
                list.Add(p);
            }

            MyCom.Dispose();
            conn.Close();

            return list;
        }

        public static List<Player> getAllOwnedPlayers(int tid)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            List<Player> list = new List<Player>();
            MySqlConnection conn = new MySqlConnection(conStr);
            MySqlCommand MyCom = new MySqlCommand("SELECT * FROM `Player` WHERE `TeamID` = @TID", conn);
            MyCom.Parameters.AddWithValue("@TID", tid);

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
                list.Add(p);
            }

            MyCom.Dispose();
            conn.Close();

            return list;
        }

        public static Player getSinglePlayerById(int id)
        {

            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            MySqlConnection conn = new MySqlConnection(conStr);
            MySqlCommand MyCom = new MySqlCommand("Select * from Player where id = @ID", conn);
            MyCom.Parameters.AddWithValue("@ID", id);
            conn.Open();

            MySqlDataReader reader = MyCom.ExecuteReader();

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

            MyCom.Dispose();
            conn.Close();

            return singleP;
        }

        public static bool sparaPlayer(Player p)
        {

            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            MySqlConnection conn = new MySqlConnection(conStr);
            MySqlCommand MyCom = new MySqlCommand("UPDATE `Player` SET `TeamID`='@teamID',`Number`='@number',`Firstname`='@firstname',`Lastname`='@lastname',`Role`='@role',`Power`='@power',`Price`='@price',`GamesPlayed`='@gamesplayed',`Goals`='@goals',`Shots`='@shots',`ShotsAgainst`='@shotsagainst]',`Saves`='@saves' where `ID`='@ID'", conn);
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
        public static bool sparanyPlayer(Player p)
        {

            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            MySqlConnection conn = new MySqlConnection(conStr);
            MySqlCommand MyCom = new MySqlCommand("INSERT INTO Player(teamID, number, firstname, lastname, role, power, price, gamesplayed, goals, shots, shotsagainst, saves) VALUES (@teamID, @number, @firrstname, @lastname, @role, @power, @price, @gamesplayed, @goals, @shots, @shotsagainst, @saves)", conn);
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
