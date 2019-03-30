using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace WhaleRiderSim
{
    class PhoneSoundManager : SoundManager
    {
        private Dictionary<String, SoundEffectInstance> _sounds;
        private ScreenManager _manager;

        public PhoneSoundManager(ScreenManager manager)
        {
            this._sounds = new Dictionary<string, SoundEffectInstance>();
            this._manager = manager;
        }

        public override void AddSound(string sound, bool repeat)
        {
            this._sounds.Add(sound, this._manager.Content.Load<SoundEffect>(@"Sounds\" + sound).CreateInstance());
            this._sounds[sound].IsLooped = repeat;
        }

        public override void RemoveSound(string sound)
        {
            this._sounds[sound].Stop();
            this._sounds[sound].Dispose();
            this._sounds.Remove(sound);
        }

        public override void PlaySound(string sound)
        {
            this._sounds[sound].Volume = Math.Sign(ConfigurationManager.GetInstance().Volume);
            this._sounds[sound].Play();
        }

        public override void StopSound(string sound)
        {
            this._sounds[sound].Stop();
        }
    }
}
