using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhaleRiderSim
{
    public enum Platform
    {
        Windows,
        WindowsPhone,
        Surface
    }

    public class HighScore
    {
        int _score;
        String _name;
        Platform _platform;
        String _gametitle;
        bool _transmitted;
        int _mode;

        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Platform PlatformID
        {
            get { return this._platform; }
            set { this._platform = value; }
        }

        public String GameTitle
        {
            get { return this._gametitle; }
            set { this._gametitle = value; }
        }

        public Boolean Transmitted
        {
            get { return this._transmitted; }
            set { this._transmitted = value; }
        }

        public int Mode
        {
            get { return this._mode; }
            set { this._mode = value; }
        }

        public static HighScore CreateFromString(String s)
        {
            List<String> hsInfo = s.Split(':').ToList();
            HighScore score = new HighScore();
            score.GameTitle = hsInfo[0];
            score.Name = hsInfo[1];
            score.PlatformID = (Platform)int.Parse(hsInfo[2]);
            score.Score = int.Parse(hsInfo[3]);
            score.Mode = int.Parse(hsInfo[4]);
            return score;
        }

        public static int Compare(HighScore h1, HighScore h2)
        {
            return h2.Score - h1.Score;
        }

        public String ToString()
        {
            return String.Format("{0}:{1}:{2}:{3}:{4}:{5}", this.GameTitle, this.Name, (int) this.PlatformID, this.Score, this.Transmitted, this.Mode);
        }
    }
}
