using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace ServiceClientPrototype
{
    class Program
    {
        static public List<String> GetHighScoresGlobally(String gameTitle)
        {
            List<String> lst = new List<String>();
            SqlConnection myConn = new SqlConnection("User Id=highScore; Password=Clt1B5aJX1IYKz4v92UX;server=RIEMANN;database=AppsData;connection timeout=30");
            try
            {
                myConn.Open();
                SqlParameter title = new SqlParameter("@Param1", SqlDbType.NVarChar, 50);
                title.Value = gameTitle;
                SqlCommand command = new SqlCommand("SELECT * FROM HighScores", myConn);
                command.Parameters.Add(title);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(reader["Score"].ToString());
                }
                myConn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return lst;
        }

        static void Main(string[] args)
        {
            HighScoreService.HighScoreServiceClient hssc = new HighScoreService.HighScoreServiceClient();
            HighScoreService.HighScore hs = new HighScoreService.HighScore();
            hs.Name = "TestClient";
            hs.PlatformID = HighScoreService.Platform.Windows;
            hs.Score = 1000;
            hs.GameTitle = "Testing";
            hssc.AddHighScore(hs);
            hs.Name = "TestClient2";
            hs.PlatformID = HighScoreService.Platform.Windows;
            hs.Score = 1000;
            hs.GameTitle = "Testing2";
            hssc.AddHighScore(hs);
            hs.Name = "TestClient3";
            hs.PlatformID = HighScoreService.Platform.Surface;
            hs.Score = 1000;
            hs.GameTitle = "Testing";
            hssc.AddHighScore(hs);
            List<HighScoreService.HighScore> lst = hssc.GetHighScoresGlobally("Testing").ToList();
            Console.WriteLine("All High Scores for Game Testing: ");
            foreach (HighScoreService.HighScore score in lst)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}", score.Name, score.Score, score.PlatformID, score.GameTitle);
            }
            Console.WriteLine("All High Scores for Windows Version of Game Testing: ");
            lst = hssc.GetHighScoresForPlatform(HighScoreService.Platform.Windows, "Testing").ToList();
            foreach (HighScoreService.HighScore score in lst)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}", score.Name, score.Score, score.PlatformID, score.GameTitle);
            }
            Console.ReadLine();
        }
    }
}
