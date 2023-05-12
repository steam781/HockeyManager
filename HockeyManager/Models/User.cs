using HockeyManager.Models;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HockeyManager.Models
{
    public class User
    {
        public int ID { get; set; } = 0;
        public int TeamID { get; set; } = 0;
        public string Role { get; set; } = "";

        [Required(ErrorMessage = "Ange valid mailadress")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail har felaktigt format")]
        [Display(Name = "Din mailadress")]
        public string Email { get; set; } = "";

        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Ange lösenord")]
        [Display(Name = "Ditt lösenord")]
        public string Password { get; set; } = "";
        public int Currency { get; set; } = 0;

        public static User GetUserByMail(string mail)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";
            MySqlConnection conn = new MySqlConnection(conStr);
            MySqlCommand MyCom = new MySqlCommand("SELECT * FROM User LEFT JOIN Team ON User.ID = Team.OwnerID where Email = @MAIL", conn);
            MyCom.Parameters.AddWithValue("@MAIL", mail);
            conn.Open();
            MySqlDataReader reader = MyCom.ExecuteReader();
            User singleE = new User();
            if (reader.Read())
            {
                singleE.ID = reader.GetInt32("ID");
                singleE.TeamID = reader.GetInt32("TeamID");
                singleE.Role = reader.GetString("Role");
                singleE.Email = reader.GetString("Email");
                singleE.Username = reader.GetString("Username");
                singleE.Password = reader.GetString("Password");
                singleE.Currency = reader.GetInt32("Currency");
            }
            else
            {

            }
            reader.Close();
            MyCom.Dispose();
            conn.Close();
            return singleE;
        }
    }
}