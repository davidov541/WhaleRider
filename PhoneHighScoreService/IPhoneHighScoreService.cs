using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PhoneHighScoreService
{
    [ServiceContract]
    public interface IPhoneHighScoreService
    {
        [OperationContract]
        void AddHighScore_Phone(String s);

        [OperationContract]
        String GetHighScoresGlobally_Phone(String gameTitle);

        [OperationContract]
        String GetHighScoresForPlatform_Phone(Platform p, String gameTitle);
    }

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
    }
}
