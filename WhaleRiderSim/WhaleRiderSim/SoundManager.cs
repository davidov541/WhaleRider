using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhaleRiderSim
{
    abstract class SoundManager
    {
        private static SoundManager _instance;

        public static SoundManager GetInstance(ScreenManager manager)
        {
            if (_instance == null)
            {
#if WINDOWS_PHONE
                _instance = new PhoneSoundManager(manager);
#elif WINDOWS
                _instance = new WindowsSoundManager(manager);
#elif SURFACE
                _instance = null;
#endif
            }
            return _instance;
        }

        public abstract void AddSound(String sound, bool repeat);

        public abstract void RemoveSound(String sound);

        public abstract void PlaySound(String sound);

        public abstract void StopSound(String sound);

        public abstract void SetVolume();
    }
}
