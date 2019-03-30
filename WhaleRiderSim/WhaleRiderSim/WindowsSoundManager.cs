using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace WhaleRiderSim
{
    class WindowsSoundManager : SoundManager
    {
        private Dictionary<String, SoundEffectInstance> _sounds;
        private ScreenManager _manager;

        public WindowsSoundManager(ScreenManager manager)
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
            this._sounds[sound].Volume = ConfigurationManager.GetInstance().Volume / 10.0f;
            this._sounds[sound].Play();
        }

        public override void StopSound(string sound)
        {
            this._sounds[sound].Stop();
        }

        public override void SetVolume()
        {
            foreach (SoundEffectInstance sound in this._sounds.Values)
            {
                sound.Volume = ConfigurationManager.GetInstance().Volume / 10.0f;
            }
        }
    }
}
