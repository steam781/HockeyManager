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
        public int LFPID { get; set; } = 0;
        //LFPID = LeftForwardPlayerID 
        public int RFPID { get; set; } = 0;
        //RFPID = RightForwardPlayerID 
        public int CPID { get; set; } = 0;
        //CPID = CenterPlayerID
        public int LDPID { get; set; } = 0;
        //LDPID = LeftDefenderPlayerID
        public int RDPID { get; set; } = 0;
        //RDPID = RightDefenderPlayerID
        public int GPID { get; set; } = 0;
        //GPID = GoliePlayerID
        public int GamesPlayed { get; set; } = 0;
        public int Goals { get; set; } = 0;
        public int GoalsAgainst { get; set; } = 0;
        public int Shots { get; set; } = 0;
        public int ShotsAgainst { get; set; } = 0;
        public int TeamID { get; set; } = 0;

        public static List<MyTeam> GetTeamInfo(int teamID)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            List<MyTeam> TeamInfo = new List<MyTeam>();
            MySqlConnection conn = new MySqlConnection(conStr);
            MySqlCommand MyCom = new MySqlCommand("SELECT * FROM `Team` WHERE `TeamID` = @teamID", conn);

            conn.Open();

            MySqlDataReader reader = MyCom.ExecuteReader();

            while (reader.Read())
            {
                MyTeam t = new MyTeam();
                t.TeamID = reader.GetInt32("teamID");
                t.OwnerID = reader.GetInt32("ownerID");
                t.Name = reader.GetString("name");
                t.Logocode = reader.GetString("logocode");
                t.LFPID = reader.GetInt32("lfpid");
                t.RFPID = reader.GetInt32("rfpid");
                t.CPID = reader.GetInt32("cpid");
                t.LDPID = reader.GetInt32("ldpid");
                t.RDPID = reader.GetInt32("rdpid");
                t.GPID = reader.GetInt32("gpid");
                t.GamesPlayed = reader.GetInt32("gamesplayed");
                t.Goals = reader.GetInt32("goals");
                t.GoalsAgainst = reader.GetInt32("goalsAgainst");
                t.Shots = reader.GetInt32("shots");
                t.ShotsAgainst = reader.GetInt32("shotsagainst");
                TeamInfo.Add(t);
            }

            MyCom.Dispose();
            conn.Close();

            return TeamInfo;
        }

        public static List<MyTeam> GetTeamPlayerInfo(int teamID)
        {
            string conStr = "server=46.246.45.183;user=OliverEc;port=3306;database=HockeyManager_OE;password=YROSBKEE";

            List<MyTeam> TeamInfo = new List<MyTeam>();
            MySqlConnection conn = new MySqlConnection(conStr);
            MySqlCommand MyCom = new MySqlCommand("SELECT * FROM `Team` LEFT JOIN Player ON Team.TeamID = Player.TeamID WHERE `TeamID` = @teamID", conn);

            conn.Open();

            MySqlDataReader reader = MyCom.ExecuteReader();

            while (reader.Read())
            {
                MyTeam t = new MyTeam();
                t.TeamID = reader.GetInt32("teamID");
                t.OwnerID = reader.GetInt32("ownerID");
                t.Name = reader.GetString("name");
                t.Logocode = reader.GetString("logocode");
                t.LFPID = reader.GetInt32("lfpid");
                t.RFPID = reader.GetInt32("rfpid");
                t.CPID = reader.GetInt32("cpid");
                t.LDPID = reader.GetInt32("ldpid");
                t.RDPID = reader.GetInt32("rdpid");
                t.GPID = reader.GetInt32("gpid");
                t.GamesPlayed = reader.GetInt32("gamesplayed");
                t.Goals = reader.GetInt32("goals");
                t.GoalsAgainst = reader.GetInt32("goalsAgainst");
                t.Shots = reader.GetInt32("shots");
                t.ShotsAgainst = reader.GetInt32("shotsagainst");
                TeamInfo.Add(t);
            }

            MyCom.Dispose();
            conn.Close();

            return TeamInfo;
        }

    }

}