using HockeyManager.Models;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;

namespace HockeyManager.Models
{
    public class MyTeam
    {
        public int OwnerID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Logocode { get; set; } = "";
        public int LF { get; set; } = 0;
        //LF = LeftForwardPlayerID 
        public int RF { get; set; } = 0;
        //RF = LeftForwardPlayerID 
        public int C { get; set; } = 0;
        //C = CenterPlayerID
        public int LD { get; set; } = 0;
        //LD = LeftDefenderPlayerID
        public int RD { get; set; } = 0;
        //RD = RightDefenderPlayerID
        public int G { get; set; } = 0;
        //G = GoliePlayerID
        public int GamesPlayed { get; set; } = 0;
        public int Goals { get; set; } = 0;
        public int GoalsAgainst { get; set; } = 0;
        public int Shots { get; set; } = 0;
        public int ShotsAgainst { get; set; } = 0;
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public List<Player> TeamPlayers { get; set; }
        public List<Player> TeamPlayerRoles { get; set; }

        public int ID { get; set; } = 0;
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


        public static MyTeam GetSingleTeamInfo(int teamID)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            MySqlConnection conn = new MySqlConnection(conStr);
            MySqlCommand MyCom = new MySqlCommand("SELECT * FROM Player LEFT JOIN Team ON Player.ID = Team.LeftForwardPlayerID OR Player.ID = Team.RightForwardPlayerID OR Player.ID = Team.CenterPlayerID OR Player.ID = Team.LeftDefenderPlayerID OR Player.ID = Team.RightDefenderPlayerID OR Player.ID = Team.GoliePlayerID WHERE Player.TeamID = @teamID", conn);
            MyCom.Parameters.AddWithValue("@teamID", teamID);

            conn.Open();

            MySqlDataReader reader = MyCom.ExecuteReader();

            MyTeam teamInfo = null;
            if (reader.Read())
            {
                teamInfo = new MyTeam();
                teamInfo.TeamID = reader.GetInt32("teamID");
                teamInfo.OwnerID = reader.GetInt32("ownerID");
                teamInfo.Name = reader.GetString("name");
                teamInfo.Logocode = reader.GetString("logocode");
                teamInfo.LF = reader.GetInt32("LeftForwardPlayerID");
                teamInfo.RF = reader.GetInt32("RightForwardPlayerID");
                teamInfo.C = reader.GetInt32("CenterPlayerID");
                teamInfo.LD = reader.GetInt32("LeftDefenderPlayerID");
                teamInfo.RD = reader.GetInt32("RightDefenderPlayerID");
                teamInfo.G = reader.GetInt32("GoliePlayerID");
                teamInfo.GamesPlayed = reader.GetInt32("gamesplayed");
                teamInfo.Goals = reader.GetInt32("goals");
                teamInfo.GoalsAgainst = reader.GetInt32("goalsAgainst");
                teamInfo.Shots = reader.GetInt32("shots");
                teamInfo.ShotsAgainst = reader.GetInt32("shotsagainst");

                teamInfo.TeamPlayerRoles = new List<Player>();

                foreach (MyTeam playerPosition in GetTeamPlayerPositions(teamID))
                {
                    Player player = new Player()
                    {
                        ID = playerPosition.ID,
                        firstname = playerPosition.firstname,
                        lastname = playerPosition.lastname,
                        number = playerPosition.number,
                        role = playerPosition.role,
                        // Assign other properties as needed
                    };

                    teamInfo.TeamPlayerRoles.Add(player);
                }
            }

            MyCom.Dispose();
            conn.Close();

            return teamInfo;
        }

        public static List<MyTeam> GetTeamPlayerPositions(int teamID)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            List<MyTeam> TeamInfo = new List<MyTeam>();
            MySqlConnection conn = new MySqlConnection(conStr);
            MySqlCommand MyCom = new MySqlCommand("SELECT Player.ID, Player.firstname, Player.lastname, Player.number, Player.role FROM Player LEFT JOIN Team AS TeamLF ON Player.ID = TeamLF.LeftForwardPlayerID LEFT JOIN Team AS TeamC ON Player.ID = TeamC.CenterPlayerID LEFT JOIN Team AS TeamRF ON Player.ID = TeamRF.LeftForwardPlayerID LEFT JOIN Team AS TeamLD ON Player.ID = TeamLD.LeftDefenderPlayerID LEFT JOIN Team AS TeamRD ON Player.ID = TeamRD.RightDefenderPlayerID LEFT JOIN Team AS TeamG ON Player.ID = TeamG.GoliePlayerID WHERE TeamLF.TeamID = @teamID OR TeamC.TeamID = @teamID OR TeamRF.TeamID = @teamID OR TeamLD.TeamID = @teamID OR TeamRD.TeamID = @teamID OR TeamG.TeamID = @teamID", conn);

            // Add the parameter to the command
            MyCom.Parameters.AddWithValue("@teamID", teamID);

            conn.Open();

            MySqlDataReader reader = MyCom.ExecuteReader();

            while (reader.Read())
            {
                MyTeam t = new MyTeam();
                t.ID = reader.GetInt32("ID");
                t.firstname = reader.GetString("firstname");
                t.lastname = reader.GetString("lastname");
                t.number = reader.GetInt32("number");
                t.role = reader.GetString("role");
                TeamInfo.Add(t);
            }

            MyCom.Dispose();
            conn.Close();

            return TeamInfo;
        }



        public static void SaveTeamPositions(int teamID, MyTeam t)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                conn.Open();

                MySqlCommand updateCommand = new MySqlCommand(
                    "UPDATE `Team` SET " +
                    "`LeftForwardPlayerID` = (CASE WHEN `TeamID` = @teamID THEN @lfpid ELSE `LeftForwardPlayerID` END), " +
                    "`LeftForwardPlayerID` = (CASE WHEN `TeamID` = @teamID THEN @rfpid ELSE `LeftForwardPlayerID` END), " +
                    "`CenterPlayerID` = (CASE WHEN `TeamID` = @teamID THEN @cpid ELSE `CenterPlayerID` END), " +
                    "`LeftDefenderPlayerID` = (CASE WHEN `TeamID` = @teamID THEN @ldpid ELSE `LeftDefenderPlayerID` END), " +
                    "`RightDefenderPlayerID` = (CASE WHEN `TeamID` = @teamID THEN @rdpid ELSE `RightDefenderPlayerID` END), " +
                    "`GoliePlayerID` = (CASE WHEN `TeamID` = @teamID THEN @gpid ELSE `GoliePlayerID` END) " +
                    "WHERE `TeamID` = @teamID", conn);

                updateCommand.Parameters.Clear();
                updateCommand.Parameters.AddWithValue("@lfpid", t.LF);
                updateCommand.Parameters.AddWithValue("@rfpid", t.RF);
                updateCommand.Parameters.AddWithValue("@cpid", t.C);
                updateCommand.Parameters.AddWithValue("@ldpid", t.LD);
                updateCommand.Parameters.AddWithValue("@rdpid", t.RD);
                updateCommand.Parameters.AddWithValue("@gpid", t.G);
                updateCommand.Parameters.AddWithValue("@teamID", teamID);

                updateCommand.ExecuteNonQuery();
            }
        }
    }
}