using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace HighScoreService
{
    public class HighScoreService : IHighScoreService
    {
        private const String _connstring = "User Id=highScore; Password=Vh5135;server=CAUCHY\\SQLSERVER;database=AppsData;connection timeout=30";

        public void AddHighScore(HighScore hs)
        {
            SqlConnection myConn = new SqlConnection(HighScoreService._connstring);
            try
            {
                myConn.Open();
                SqlParameter name = new SqlParameter("@Param1", SqlDbType.NVarChar, 50);
                name.Value = hs.Name;
                SqlParameter score = new SqlParameter("@Param2", SqlDbType.Int, 6);
                score.Value = hs.Score;
                SqlParameter platform = new SqlParameter("@Param3", SqlDbType.SmallInt, 2);
                platform.Value = (int)hs.PlatformID;
                SqlParameter game = new SqlParameter("@Param4", SqlDbType.NVarChar, 50);
                game.Value = hs.GameTitle;
                SqlCommand command = new SqlCommand("INSERT INTO HighScores VALUES (@Param1, @Param2, @Param3, @Param4)", myConn);
                command.Parameters.Add(name);
                command.Parameters.Add(score);
                command.Parameters.Add(platform);
                command.Parameters.Add(game);
                command.ExecuteNonQuery();
                myConn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void AddHighScore_Phone(String s)
        {
            List<String> hses = s.Split('\n').ToList();
            foreach (String hs in hses)
            {
                List<String> hsInfo = hs.Split(':').ToList();
                HighScore score = new HighScore();
                score.GameTitle = hsInfo[0];
                score.Name = hsInfo[1];
                score.PlatformID = (Platform) int.Parse(hsInfo[2]);
                score.Score = int.Parse(hsInfo[3]);
                this.AddHighScore(score);
            }
        }

        public List<HighScore> GetHighScoresGlobally(String gameTitle)
        {
            List<HighScore> lst = new List<HighScore>();
            SqlConnection myConn = new SqlConnection(HighScoreService._connstring);
            try
            {
                myConn.Open();
                SqlParameter title = new SqlParameter("@Param1", SqlDbType.NVarChar, 50);
                title.Value = gameTitle;
                SqlCommand command = new SqlCommand("SELECT * FROM HighScores WHERE GameTitle = @Param1", myConn);
                command.Parameters.Add(title);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    HighScore hs = new HighScore();
                    hs.Name = reader["PlayerName"].ToString();
                    hs.Score = int.Parse(reader["Score"].ToString());
                    hs.PlatformID = (Platform) int.Parse(reader["Platform"].ToString());
                    hs.GameTitle = reader["GameTitle"].ToString();
                    lst.Add(hs);
                }
                var scoreLst = from item in lst orderby item.Score descending select item;
                lst = new List<HighScore>(scoreLst.Take(5));
                myConn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return lst;
        }

        public String GetHighScoresGlobally_Phone(String gameTitle)
        {
            List<HighScore> hses = this.GetHighScoresGlobally(gameTitle);
            String result = "";
            foreach (HighScore hs in hses)
            {
                result += String.Format("{0}:{1}:{2}:{3}\n", hs.GameTitle, hs.Name, hs.PlatformID, hs.Score);
            }
            return result;
        }

        public List<HighScore> GetHighScoresForPlatform(Platform p, String gameTitle)
        {
            List<HighScore> lst = new List<HighScore>();
            SqlConnection myConn = new SqlConnection(HighScoreService._connstring);
            try
            {
                myConn.Open();
                SqlParameter title = new SqlParameter("@Param1", SqlDbType.NVarChar, 50);
                title.Value = gameTitle;
                SqlParameter platform = new SqlParameter("@Param2", SqlDbType.SmallInt, 4);
                platform.Value = (int) p;
                SqlCommand command = new SqlCommand("SELECT * FROM HighScores WHERE GameTitle = @Param1 AND Platform = @Param2", myConn);
                command.Parameters.Add(title);
                command.Parameters.Add(platform);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    HighScore hs = new HighScore();
                    hs.Name = reader["PlayerName"].ToString();
                    hs.Score = int.Parse(reader["Score"].ToString());
                    hs.PlatformID = (Platform)int.Parse(reader["Platform"].ToString());
                    hs.GameTitle = reader["GameTitle"].ToString();
                    lst.Add(hs);
                }
                var scoreLst = from item in lst orderby item.Score descending select item;
                lst = new List<HighScore>(scoreLst.Take(5));
                myConn.Close();
            }
            catch (Exception e)
            {
                HighScore hs = new HighScore();
                hs.Name = e.ToString();
                lst.Add(hs);
            }
            return lst;
        }

        public String GetHighScoresForPlatform_Phone(Platform p, String gameTitle)
        {
            List<HighScore> hses = this.GetHighScoresForPlatform(p, gameTitle);
            String result = "";
            foreach (HighScore hs in hses)
            {
                result += String.Format("{0}:{1}:{2}:{3}\n", hs.GameTitle, hs.Name, hs.PlatformID, hs.Score);
            }
            return result;
        }
    }
}
