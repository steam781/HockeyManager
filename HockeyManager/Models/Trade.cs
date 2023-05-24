using HockeyManager.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;

namespace HockeyManager.Models
{
    public class Trade
    {
        public int ID { get; set; } = 0;
        public int teamID { get; set; } = 0;
        public int playerID { get; set; } = 0;
        public DateTime date { get; set; }
        public int type { get; set; } = 0;
        public int oldprice { get; set; } = 0;
        public int number { get; set; } = 0;
        public string firstname { get; set; } = "";
        public string lastname { get; set; } = "";
        public string role { get; set; } = "";
        public int power { get; set; } = 0;
        public int price { get; set; } = 0;
        public int priceDiffrence { get; set; } = 0;
        public List<Trade> Trades { get; set; }
    }

    public static class TradeManager
    {
        public static Trade getAllTrades(int teamID)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            List<Trade> TradesT = new List<Trade>();

            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                conn.Open();

                MySqlCommand TradeCmd = new MySqlCommand("SELECT Trade.*, Player.Number, Player.Firstname, Player.Lastname, Player.Role, Player.Power, Player.Price FROM `Trade` LEFT JOIN Player ON Trade.playerID = Player.ID WHERE Trade.TeamID = @teamID", conn);
                TradeCmd.Parameters.AddWithValue("@teamID", teamID);

                MySqlDataReader reader = TradeCmd.ExecuteReader();
                while (reader.Read())
                {
                    Trade t = new Trade();
                    t.ID = reader.GetInt32("ID");
                    t.teamID = reader.GetInt32("teamID");
                    t.playerID = reader.GetInt32("playerID");
                    t.date = reader.GetDateTime("date");
                    t.type = reader.GetInt32("type");
                    t.oldprice = reader.GetInt32("oldprice");
                    t.number = reader.GetInt32("number");
                    t.firstname = reader.GetString("firstname");
                    t.lastname = reader.GetString("lastname");
                    t.role = reader.GetString("role");
                    t.power = reader.GetInt32("power");
                    t.price = reader.GetInt32("price");

                    t.priceDiffrence = t.price - t.oldprice;

                    TradesT.Add(t);
                }
                reader.Close();

                TradeCmd.Dispose();
                conn.Close();
            }

            return new Trade { Trades = TradesT}; ;
        }


        public static Trade getSingleTradeById(int id)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                conn.Open();

                using (MySqlCommand MyCom = new MySqlCommand("SELECT * FROM Trade WHERE id = @ID", conn))
                {
                    MyCom.Parameters.AddWithValue("@ID", id);

                    using (MySqlDataReader reader = MyCom.ExecuteReader())
                    {
                        Trade singleT = new Trade();

                        if (reader.Read())
                        {
                            singleT.ID = reader.GetInt32("ID");
                            singleT.teamID = reader.GetInt32("teamID");
                            singleT.playerID = reader.GetInt32("playerID");
                            singleT.date = reader.GetDateTime("date");
                            singleT.type = reader.GetInt32("type");
                            singleT.oldprice = reader.GetInt32("oldprice");
                        }

                        return singleT;
                    }
                }
            }
        }

        public static bool saveNewTrade(Trade t)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            MySqlConnection conn = new MySqlConnection(conStr);
            MySqlCommand MyCom = new MySqlCommand("INSERT INTO Trade(teamID, playerID, date, type, oldprice, ID) VALUES (@teamID, @playerID, @type, @oldprice)", conn);
            MyCom.Parameters.AddWithValue("@teamID", t.teamID);
            MyCom.Parameters.AddWithValue("@playerID", t.playerID);
            MyCom.Parameters.AddWithValue("@type", t.type);
            MyCom.Parameters.AddWithValue("@oldprice", t.oldprice);
            conn.Open();

            int rowsAffected = MyCom.ExecuteNonQuery();

            MyCom.Dispose();
            conn.Close();

            if (rowsAffected == 0)
                return false;
            else
                return true;
        }


    }

}