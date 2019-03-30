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

        public abstract String PlatformName
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
            this.SetNewSettings(locale, this.Volume);
        }

        public abstract void SetNewSettings(String language, float volume);

        public abstract void AddNewScore(String name, int score);

        public abstract List<HighScore> GetLocalHighScores();

        public abstract List<HighScore> StartGetPlatformHighScores(AsyncReturn d);

        public abstract List<HighScore> StartGetGlobalHighScores(AsyncReturn d);
        #endregion
    }
}
