using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HighScoreService
{
    [ServiceContract]
    public interface IHighScoreService
    {
        [OperationContract]
        void AddHighScore(HighScore hs);

        [OperationContract]
        List<HighScore> GetHighScoresGlobally(String gameTitle);

        [OperationContract]
        List<HighScore> GetHighScoresForPlatform(Platform p, String gameTitle);
    }

    [DataContract(Name = "Platform")]
    public enum Platform
	{
	    [EnumMember]
        Windows,
        [EnumMember]
        WindowsPhone,
        [EnumMember]
        Surface
	}

    [DataContract]
    public class HighScore
    {
        int _score;
        String _name;
        Platform _platform;
        String _gametitle;
        bool _transmitted;

        [DataMember]
        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        [DataMember]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [DataMember]
        public Platform PlatformID
        {
            get { return this._platform; }
            set { this._platform = value; }
        }

        [DataMember]
        public String GameTitle
        {
            get { return this._gametitle; }
            set { this._gametitle = value; }
        }

        [DataMember]
        public Boolean Transmitted
        {
            get { return this._transmitted; }
            set { this._transmitted = value; }
        }
    }
}
