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

        public static User GetEmployeeByMail(string mail)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=OliverEc_DB;password=YROSBKEE";
            MySqlConnection conn = new MySqlConnection(conStr);
            MySqlCommand MyCom = new MySqlCommand("Select * from User where email = @MAIL", conn);
            MyCom.Parameters.AddWithValue("@MAIL", mail);
            conn.Open();
            MySqlDataReader reader = MyCom.ExecuteReader();
            User singleU = new User();
            if (reader.Read())
            {
                singleU.ID = reader.GetInt32("id");
                singleU.Role = reader.GetString("role");
                singleU.Email = reader.GetString("email");
                singleU.Username = reader.GetString("username");
                singleU.Password = reader.GetString("password");
                singleU.Currency = reader.GetString("currency");
            }
            else
            {

            }
            reader.Close();
            MyCom.Dispose();
            conn.Close();
            return singleU;
        }
    }

}
