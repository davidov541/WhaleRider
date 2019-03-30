using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhaleRiderSim
{
    class HighScoreManager
    {
        private static HighScoreManager _instance;
        private HighScoreManager()
        {

        }

        public static HighScoreManager GetInstance()
        {
            if(HighScoreManager._instance == null)
            {
                HighScoreManager._instance = new HighScoreManager();
            }
            return HighScoreManager._instance;
        }

        public String GetGlobalScores(String gametitle)
        {
            PHighScoreService.PhoneHighScoreServiceClient phssc = new PHighScoreService.PhoneHighScoreServiceClient();
            phssc.GetHighScoresGlobally_PhoneCompleted += new EventHandler<PHighScoreService.GetHighScoresGlobally_PhoneCompletedEventArgs>(phssc_GetHighScoresGlobally_PhoneCompleted);
            phssc.GetHighScoresGlobally_PhoneAsync(gametitle);
            return "";
        }

        void phssc_GetHighScoresGlobally_PhoneCompleted(object sender, PHighScoreService.GetHighScoresGlobally_PhoneCompletedEventArgs e)
        {
            int x = 10;
        }
    }
}
