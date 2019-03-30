using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using Microsoft.Xna.Framework.GamerServices;
using System.IO.IsolatedStorage;
using System.Windows;
using System.ServiceModel;

namespace WhaleRiderSim
{
    class PhoneConfigurationManager : ConfigurationManager
    {

        #region Properties
        private IsolatedStorageSettings _usersettings;
        private AsyncReturn _returnDel;

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
                return new CultureInfo(this._usersettings["LanguageCode"].ToString()); 
            }
        }

        //the game volume
        public override float Volume
        {
            get 
            { 
                return (float) this._usersettings["VolumeLevel"]; 
            }
        }

        public override bool Train
        {
            get 
            { 
                return (Boolean) this._usersettings["TrainingMode"]; 
            }
        }

        public override String PlatformName
        {
            get
            {
                return "WP7";
            }
        }

        public override bool HasShownTrainMode
        {
            get { return (Boolean)this._usersettings["HasShownTrainMode"]; }
        }

        private const String SVCPATH = "http://tfs.davidmcginnisonline.com:5135/AppServices/PhoneHighScoreService/PhoneHighScoreService.svc";
#endregion

        #region Functions
        public PhoneConfigurationManager()
        {
            this._resmgr = new ResourceManager("StringResources", Assembly.GetExecutingAssembly());
            LoadSettingsFile();
        }

        private void LoadSettingsFile()
        {
            this._usersettings = IsolatedStorageSettings.ApplicationSettings;
            if (!this._usersettings.Contains("VolumeLevel"))
            {
                this._usersettings.Add("VolumeLevel", 5.0f);
            }
            if (!this._usersettings.Contains("LanguageCode"))
            {
                this._usersettings.Add("LanguageCode", "en-US");
            }
            if (!this._usersettings.Contains("HighScores"))
            {
                this._usersettings.Add("HighScores", new List<HighScore>());
            }
            if (!this._usersettings.Contains("TrainingMode"))
            {
                this._usersettings.Add("TrainingMode", false);
            }
            if (!this._usersettings.Contains("HasShownTrainMode"))
            {
                this._usersettings.Add("HasShownTrainMode", false);
            }
            if (!this._usersettings.Contains("TrainHighScores"))
            {
                this._usersettings.Add("TrainHighScores", new List<HighScore>());
            }
            this._usersettings.Save();
            Thread.CurrentThread.CurrentCulture = this.Locale;
            Thread.CurrentThread.CurrentUICulture = this.Locale;
        }

        public override void SetNewSettings(String language, float volume, Boolean trainingMode)
        {
            this._usersettings["LanguageCode"] = language;
            this._usersettings["VolumeLevel"] = volume;
            this._usersettings["TrainingMode"] = trainingMode;
            this._usersettings.Save();
            Thread.CurrentThread.CurrentCulture = this.Locale;
            Thread.CurrentThread.CurrentUICulture = this.Locale;
        }

        public override void AddNewScore(String name, int score)
        {
            List<HighScore> lst = this.GetLocalHighScores(this.Train ? 1 : 0);
            HighScore hst = new HighScore();
            hst.GameTitle = "WhaleRider";
            hst.Name = name;
            hst.Score = score;
            hst.PlatformID = Platform.WindowsPhone;
            hst.Transmitted = false;
            hst.Mode = this.Train ? 1 : 0;
            lst.Add(hst);
            lst.Sort(HighScore.Compare);
            if (lst.Count > 5)
            {
                lst.RemoveAt(5);
            }
            foreach (HighScore hs in lst)
            {
                if (!hs.Transmitted)
                {
                    PhoneHighScoreServiceClient phssc = new PhoneHighScoreServiceClient(new BasicHttpBinding(), new EndpointAddress(SVCPATH));
                    phssc.AddHighScore_PhoneCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(phssc_AddHighScore_PhoneCompleted);
                    phssc.AddHighScore_PhoneAsync(hs.ToString());
                }
            }
            if (this.Train)
            {
                IsolatedStorageSettings.ApplicationSettings["TrainHighScores"] = lst;
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings["HighScores"] = lst;
            }
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        void phssc_AddHighScore_PhoneCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                List<HighScore> lst = this.GetLocalHighScores(this.Train ? 1 : 0);
                foreach (HighScore hs in lst)
                {
                    hs.Transmitted = true;
                }
            }
        }

        public override List<HighScore> GetLocalHighScores(int mode)
        {
            if (mode == 1)
            {
                return (List<HighScore>)IsolatedStorageSettings.ApplicationSettings["TrainHighScores"];
            }
            else
            {
                return (List<HighScore>)IsolatedStorageSettings.ApplicationSettings["HighScores"];
            }
        }

        public override List<HighScore> StartGetPlatformHighScores(AsyncReturn d, int mode)
        {
            this._returnDel = d;

            PhoneHighScoreServiceClient phssc = new PhoneHighScoreServiceClient(new BasicHttpBinding(), new EndpointAddress(SVCPATH));
            phssc.GetHighScoresForPlatformv2_PhoneCompleted += new EventHandler<GetHighScoresForPlatformv2_PhoneCompletedEventArgs>(phssc_GetHighScoresForPlatform_PhoneCompleted);
            phssc.GetHighScoresForPlatformv2_PhoneAsync(PhoneHighScoreService.Platform.WindowsPhone, "WhaleRider", mode);
            return new List<HighScore>();
        }

        void phssc_GetHighScoresForPlatform_PhoneCompleted(object sender, GetHighScoresForPlatformv2_PhoneCompletedEventArgs e)
        {
            try
            {
                List<HighScore> scores = new List<HighScore>();
                foreach (String score in e.Result.Split('\n'))
                {
                    if (score.Length > 0)
                    {
                        scores.Add(HighScore.CreateFromString(score));
                    }
                }
                this._returnDel(scores);
            }
            catch (Exception exp)
            {
                Console.Error.WriteLine(exp.Message);
                this._returnDel(null);
            }
        }

        public override List<HighScore> StartGetGlobalHighScores(AsyncReturn d, int mode)
        {
            this._returnDel = d;
            PhoneHighScoreServiceClient phssc = new PhoneHighScoreServiceClient(new BasicHttpBinding(), new EndpointAddress(SVCPATH));
            phssc.GetHighScoresGloballyv2_PhoneCompleted += new EventHandler<GetHighScoresGloballyv2_PhoneCompletedEventArgs>(phssc_GetHighScoresGlobally_PhoneCompleted);
            phssc.GetHighScoresGloballyv2_PhoneAsync("WhaleRider", mode);
            return new List<HighScore>();
        }

        void phssc_GetHighScoresGlobally_PhoneCompleted(object sender, GetHighScoresGloballyv2_PhoneCompletedEventArgs e)
        {
            try
            {
                List<HighScore> scores = new List<HighScore>();
                foreach (String score in e.Result.Split('\n'))
                {
                    if (score.Length > 0)
                    {
                        scores.Add(HighScore.CreateFromString(score));
                    }
                }
                this._returnDel(scores);
            }
            catch (Exception)
            {
                this._returnDel(null);
            }
        }
        #endregion
    }
}
