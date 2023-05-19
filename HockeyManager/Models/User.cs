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
        public static User GetUserByID(int id)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";
            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                conn.Open();

                MySqlCommand command = new MySqlCommand("SELECT * FROM User LEFT JOIN Team ON User.ID = Team.OwnerID WHERE User.ID = @ID", conn);
                command.Parameters.AddWithValue("@ID", id);

                MySqlDataReader reader = command.ExecuteReader();

                User user = null;
                if (reader.Read())
                {
                    user = new User
                    {
                        ID = reader.GetInt32("ID"),
                        TeamID = reader.GetInt32("TeamID"),
                        Role = reader.GetString("Role"),
                        Email = reader.GetString("Email"),
                        Username = reader.GetString("Username"),
                        Password = reader.GetString("Password"),
                        Currency = reader.GetInt32("Currency")
                    };
                }

                reader.Close();
                conn.Close();

                return user;
            }
        }
        public static void DecreaseCurrency(int userID, int amount)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";
            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                conn.Open();

                MySqlCommand command = new MySqlCommand("UPDATE `User` SET `Currency` = `Currency` - @amount WHERE `ID` = @ID", conn);
                command.Parameters.AddWithValue("@amount", amount);
                command.Parameters.AddWithValue("@ID", userID);

                int rader = command.ExecuteNonQuery();

                command.Dispose();
                conn.Close();
            }
        }

        public static void IncreaseCurrency(int userID, int amount)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";
            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                conn.Open();

                MySqlCommand command = new MySqlCommand("UPDATE `User` SET `Currency` = `Currency` + @amount WHERE `ID` = @ID");
                command.Parameters.AddWithValue("@amount", amount);
                command.Parameters.AddWithValue("@ID", userID);
                command.Connection = conn;

                int rader = command.ExecuteNonQuery();

                command.Dispose();
                conn.Close();
            }
        }

        public static bool Register(User u, MyTeam t)
        {
            User newUser = HockeyManager.Models.User.GetUserByMail(u.Email);


            // Check if user already exists
            if (newUser.Email == u.Email)
            {
                return false;
            }
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";
            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                conn.Open();

                MySqlCommand UserCmd = new MySqlCommand("INSERT INTO User(Email, Username, Password) VALUES (@MAIL, @USER, @PASS)", conn);

                UserCmd.Parameters.AddWithValue("@MAIL", u.Email);
                UserCmd.Parameters.AddWithValue("@USER", u.Username);
                UserCmd.Parameters.AddWithValue("@PASS", u.Password);

                int rowsAffected = UserCmd.ExecuteNonQuery();

                UserCmd.Dispose();

                MySqlCommand TeamCmd = new MySqlCommand("INSERT INTO Team(OwnerID, Name) VALUES (@OID, @NAME)", conn);

                TeamCmd.Parameters.AddWithValue("@OID", u.ID);
                TeamCmd.Parameters.AddWithValue("@NAME", u.Username + "'s Team");

                rowsAffected = TeamCmd.ExecuteNonQuery();

                TeamCmd.Dispose();
                conn.Close();

                return true;
            }
        }
    }
}