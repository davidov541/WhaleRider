using System;
using System.Globalization;
using System.Collections.Generic;

namespace WhaleRiderSim
{
    abstract class ConfigurationManager
    {
        public delegate void AsyncReturn(List<HighScore> scores);

        #region Properties
        //the current locale
        abstract public CultureInfo Locale
        {
            get;
            //set;
        }

        //the game volume
        abstract public float Volume
        {
            get;
        }

        abstract public Boolean Train
        {
            get;
        }

        public abstract String PlatformName
        {
            get;
        }

        public abstract Boolean HasShownTrainMode
        {
            get;
        }

        private static ConfigurationManager singleton; 
        #endregion

        #region Functions
        //returns the global copy of the configuration manager
        public static ConfigurationManager GetInstance()
        {
            if (ConfigurationManager.singleton == null)
            {
#if SURFACE
                ConfigurationManager.singleton = new #SurfaceConfigurationManager();
#elif WINDOWS_PHONE
                ConfigurationManager.singleton = new PhoneConfigurationManager();
#elif WINDOWS
                ConfigurationManager.singleton = new WindowsConfigurationManager();
#endif
             }
            return ConfigurationManager.singleton;
        }

        public void SetCulture(String locale)
        {
            this.SetNewSettings(locale, this.Volume, this.Train);
        }

        public abstract void SetNewSettings(String language, float volume, Boolean train);

        public abstract void AddNewScore(String name, int score);

        public abstract List<HighScore> GetLocalHighScores(int mode);

        public abstract List<HighScore> StartGetPlatformHighScores(AsyncReturn d, int mode);

        public abstract List<HighScore> StartGetGlobalHighScores(AsyncReturn d, int mode);
        #endregion
    }
}
