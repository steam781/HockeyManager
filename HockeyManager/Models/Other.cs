using HockeyManager.Models;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HockeyManager.Models
{
    public class Other
    {
        public string SecurityKey { get; set; } = "";

        public static Other GetKey()
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";
            MySqlConnection conn = new MySqlConnection(conStr);
            MySqlCommand MyCom = new MySqlCommand("SELECT SecurityKey FROM Other", conn);
            conn.Open();
            MySqlDataReader reader = MyCom.ExecuteReader();
            Other other = new Other();
            if (reader.Read())
            {
                other.SecurityKey = reader.GetString("SecurityKey");
            }
            Console.WriteLine(conn);
            Console.WriteLine(MyCom);
            Console.WriteLine(other);
            MyCom.Dispose();
            conn.Close();
            return other;
        }
    }
}
