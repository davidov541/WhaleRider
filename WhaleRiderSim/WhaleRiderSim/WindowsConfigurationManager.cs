using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Xml;
using System.ServiceModel;

namespace WhaleRiderSim
{
    class WindowsConfigurationManager : ConfigurationManager
    {
        #region Properties
        private XmlDocument _doc;

        //uses the resource bundle for internationalization
        private ResourceManager _resmgr;
        public ResourceManager ResMgr
        {
            get 
            { 
                return _resmgr; 
            }
        }

        //the current locale
        public override CultureInfo Locale
        {
            get 
            { 
                return new CultureInfo(this._doc.GetElementsByTagName("LanguageCode")[0].InnerText); 
            }            
        }

        //the game volume
        public override float Volume
        {
            get
            {
                return float.Parse(this._doc.GetElementsByTagName("VolumeLevel")[0].InnerText);
            }
        }

        public override String PlatformName
        {
            get
            {
                return "Windows";
            }
        }
        private const String SVCPATH = "http://tfs.davidmcginnisonline.com:5135/AppServices/HighScoreService/HighScoreService.svc";
        #endregion

        #region Functions
        public WindowsConfigurationManager()
        {
            this._resmgr = new ResourceManager("StringResources", Assembly.GetExecutingAssembly());
            LoadSettingsFile();
        }

        private void LoadSettingsFile()
        {
            if (!File.Exists("settings.xml"))
            {
                XmlTextWriter xtw = new XmlTextWriter("settings.xml", System.Text.Encoding.Unicode);
                xtw.WriteStartDocument();
                xtw.WriteStartElement("Configuration");
                xtw.WriteStartElement("Settings");
                xtw.WriteStartElement("LanguageCode");
                xtw.WriteString(Thread.CurrentThread.CurrentCulture.Name);
                xtw.WriteEndElement();
                xtw.WriteStartElement("VolumeLevel");
                xtw.WriteString("5");
                xtw.WriteEndElement();
                xtw.WriteEndElement();
                xtw.WriteStartElement("HighScores");
                xtw.WriteEndElement();
                xtw.WriteEndElement();
                xtw.WriteEndDocument();
                xtw.Close();
            }
            this._doc = new XmlDocument();
            XmlTextReader reader = new XmlTextReader("settings.xml");
            reader.Read();
            this._doc.Load(reader);
            reader.Close();
            Thread.CurrentThread.CurrentCulture = this.Locale;
            Thread.CurrentThread.CurrentUICulture = this.Locale;
        }

        public override void SetNewSettings(String language, float volume)
        {
            bool volumeChanged = volume != this.Volume;
            this._doc.GetElementsByTagName("LanguageCode")[0].InnerText = language;
            this._doc.GetElementsByTagName("VolumeLevel")[0].InnerText = volume.ToString();
            XmlTextWriter xtw = new XmlTextWriter("settings.xml", System.Text.Encoding.Unicode);
            this._doc.Save(xtw);
            xtw.Close();
            Thread.CurrentThread.CurrentCulture = this.Locale;
            Thread.CurrentThread.CurrentUICulture = this.Locale;
            if (volumeChanged)
            {
                SoundManager.GetInstance(null).SetVolume();
            }
        }

        public override void AddNewScore(String name, int score)
        {
            List <HighScoreService.HighScore> scores = this.AddNewScoreLocally(name, score);

            this.AddNewScoreRemotely(scores);
        }

        private List<HighScoreService.HighScore> AddNewScoreLocally(String name, int score)
        {
            XmlNode highscoresparent = this._doc.GetElementsByTagName("HighScores")[0];
            XmlNode highscoretag = this._doc.CreateNode(XmlNodeType.Element, "HighScore", "");
            XmlNode highscorename = this._doc.CreateNode(XmlNodeType.Element, "PlayerName", "");
            highscorename.InnerText = name;
            XmlNode highscoreval = this._doc.CreateNode(XmlNodeType.Element, "Score", "");
            highscoreval.InnerText = score.ToString();
            XmlNode highscoretrans = this._doc.CreateNode(XmlNodeType.Element, "Transmitted", "");
            highscoretrans.InnerText = "false";
            highscoretag.AppendChild(highscorename);
            highscoretag.AppendChild(highscoreval);
            highscoretag.AppendChild(highscoretrans);
            highscoresparent.AppendChild(highscoretag);
            XmlNodeList highscores = this._doc.GetElementsByTagName("HighScore");
            if (highscores.Count > 5)
            {
                int minindex = 5;
                int minval = int.MaxValue;
                int i = 0;
                foreach (XmlElement el in this._doc.GetElementsByTagName("HighScore"))
                {
                    int currval = int.Parse(el["Score"].InnerText);
                    if (currval < minval)
                    {
                        minindex = i;
                        minval = currval;
                    }
                    i++;
                }

                highscoresparent.RemoveChild(this._doc.GetElementsByTagName("HighScore")[minindex]);
            }
            XmlTextWriter xtw2 = new XmlTextWriter("settings.xml", System.Text.Encoding.Unicode);
            this._doc.Save(xtw2);
            xtw2.Close();

            List<HighScoreService.HighScore> scores = new List<HighScoreService.HighScore>();
            foreach (XmlElement el in this._doc.GetElementsByTagName("HighScore"))
            {
                if (el["Transmitted"].InnerText.Equals("false"))
                {
                    HighScoreService.HighScore newscore = new HighScoreService.HighScore();
                    newscore.GameTitle = "WhaleRider";
                    newscore.Name = el["PlayerName"].InnerText;
                    newscore.PlatformID = HighScoreService.Platform.Windows;
                    newscore.Score = int.Parse(el["Score"].InnerText);
                    newscore.Transmitted = false;
                    scores.Add(newscore);
                }
            }
            return scores;
        }

        private void AddNewScoreRemotely(List<HighScoreService.HighScore> scores)
        {
            try
            {
                HighScoreService.HighScoreServiceClient hssc = new HighScoreService.HighScoreServiceClient(new BasicHttpBinding(), new EndpointAddress(SVCPATH));
                foreach (HighScoreService.HighScore score in scores)
                {
                    hssc.AddHighScore(score);
                }

                foreach (XmlElement el in this._doc.GetElementsByTagName("HighScore"))
                {
                    el["Transmitted"].InnerText = "true";
                }
                XmlTextWriter xtw2 = new XmlTextWriter("settings.xml", System.Text.Encoding.Unicode);
                this._doc.Save(xtw2);
                xtw2.Close();
            }
            catch (Exception e)
            {}
        }
        #endregion

        public override List<HighScore> GetLocalHighScores()
        {
            XmlElement xscores = this._doc["Configuration"]["HighScores"];
            List<HighScore> scores = new List<HighScore>();
            foreach (XmlNode xscore in xscores.ChildNodes)
            {
                HighScore hs = new HighScore();
                hs.GameTitle = "WhaleRider";
                hs.Name = xscore["PlayerName"].InnerText;
                hs.PlatformID = Platform.Windows;
                hs.Score = Int16.Parse(xscore["Score"].InnerText);
                hs.Transmitted = Boolean.Parse(xscore["Transmitted"].InnerText);
                scores.Add(hs);
            }
            scores.Sort(HighScore.Compare);
            return scores;
        }

        public override List<HighScore> StartGetPlatformHighScores(AsyncReturn d)
        {
            List<HighScore> scores = new List<HighScore>();
            HighScoreService.HighScoreServiceClient hssc = new HighScoreService.HighScoreServiceClient(new BasicHttpBinding(), new EndpointAddress(SVCPATH));
            foreach (HighScoreService.HighScore newscore in hssc.GetHighScoresForPlatform(HighScoreService.Platform.Windows, "WhaleRider"))
            {
                HighScore score = new HighScore();
                score.GameTitle = "WhaleRider";
                score.Name = newscore.Name;
                score.PlatformID = Platform.Windows;
                score.Score = newscore.Score;
                score.Transmitted = true;
                scores.Add(score);
            }
            return scores;
        }

        public override List<HighScore> StartGetGlobalHighScores(AsyncReturn d)
        {
            List<HighScore> scores = new List<HighScore>();
            HighScoreService.HighScoreServiceClient hssc = new HighScoreService.HighScoreServiceClient(new BasicHttpBinding(), new EndpointAddress(SVCPATH));
            foreach (HighScoreService.HighScore newscore in hssc.GetHighScoresGlobally("WhaleRider"))
            {
                HighScore score = new HighScore();
                score.GameTitle = "WhaleRider";
                score.Name = newscore.Name;
                if (newscore.PlatformID == HighScoreService.Platform.Surface)
                {
                    score.PlatformID = Platform.Surface;
                }
                else if (newscore.PlatformID == HighScoreService.Platform.Windows)
                {
                    score.PlatformID = Platform.Windows;
                }
                else if (newscore.PlatformID == HighScoreService.Platform.WindowsPhone)
                {
                    score.PlatformID = Platform.WindowsPhone;
                }
                score.Score = newscore.Score;
                score.Transmitted = true;
                scores.Add(score);
            }
            return scores;
        }
    }
}
