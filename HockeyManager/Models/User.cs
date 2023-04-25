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
        public string Role { get; set; } = "";

        [Required(ErrorMessage = "Ange valid mailadress")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail har felaktigt format")]
        [Display(Name = "Din mailadress")]
        public string Email { get; set; } = "";

        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Ange lösenord")]
        [Display(Name = "Ditt lösenord")]
        public string Password { get; set; } = "";
        public string Currency { get; set; } = "";

        public static User GetUserByMail(string mail)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=OliverEc_DB;password=YROSBKEE";
            MySqlConnection conn = new MySqlConnection(conStr);
            MySqlCommand MyCom = new MySqlCommand("Select * from Employee where mailadress = @MAIL", conn);
            MyCom.Parameters.AddWithValue("@MAIL", mail);
            conn.Open();
            MySqlDataReader reader = MyCom.ExecuteReader();
            User singleE = new User();
            if (reader.Read())
            {
                singleE.ID = reader.GetInt32("ID");
                singleE.Role = reader.GetString("Role");
                singleE.Email = reader.GetString("Email");
                singleE.Username = reader.GetString("Username");
                singleE.Password = reader.GetString("Password");
                singleE.Currency = reader.GetString("Currency");
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