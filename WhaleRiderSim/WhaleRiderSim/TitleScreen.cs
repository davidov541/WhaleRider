using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Threading;
#if WINDOWS_PHONE
using WhaleRiderForPhone;
#endif

namespace WhaleRiderSim
{
    class TitleScreen : GameScreen
    {
        #region Properties
        private Background _background;
        public Background Background
        {
            get { return _background; }
            set { _background = value; }
        }

        private Text _title;
        internal Text Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private Button _play;
        internal Button Play
        {
            get { return _play; }
            set { _play = value; }
        }

        private Button _settings;
        internal Button Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        private Button _highscore;
        internal Button HighScore
        {
            get { return _highscore; }
            set { _highscore = value; }
        }

        private Button _instructions;
        internal Button Instructions
        {
            get { return _instructions; }
            set { _instructions = value; }
        }

        private Button _volume;

        private InputHandler _ih;
        internal InputHandler IH
        {
            get { return _ih; }
            set { _ih = value; }
        } 
        #endregion

        #region Functions
        public TitleScreen()
        {
        }

        public TitleScreen(ScreenManager sm)
            : base(sm)
        {
            Texture2D button = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\buttonsexpanded");
            SpriteFont headerfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.TitleHeader_font);
            SpriteFont buttonfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Button_font);

            this.IH = InputHandler.GetInstance();
            this._background = new Background(this._screenmanager.Content.Load<Texture2D>(@"Backgrounds\titlescreenbackground"));

            this._title = new Text(StringResources.Title_Screen, 0.5f, 0.1f, headerfont, Text.Justification.Center);
            this._play = new Button(StringResources.Start, Button.Size.Medium, button, 0.05f, 0.8f, 0.25f, 0.9f, buttonfont);
#if WINDOWS || SURFACE
            this._settings = new Button(StringResources.Options, Button.Size.Medium, button, 0.28f, 0.8f, 0.48f, 0.9f, buttonfont);
            this._instructions = new Button(StringResources.Instructions, Button.Size.Medium, button, 0.53f, 0.8f, 0.73f, 0.9f, buttonfont);
#else
            this._instructions = new Button(StringResources.Instructions, Button.Size.Medium, button, 0.41f, 0.8f, 0.61f, 0.9f, buttonfont);
            Texture2D volume;
            if(ConfigurationManager.GetInstance().Volume > 0)
            {
                volume = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\volumeOn");
            }
            else
            {
                volume = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\volumeOff");
            }
            this._volume = new Button("", Button.Size.Custom, volume, 0.85f, 0.6f, 0.92f, 0.7f, buttonfont);
#endif
#if WINDOWS || WINDOWS_PHONE
            this._highscore = new Button(StringResources.HighScore, Button.Size.Medium, button, 0.77f, 0.8f, 0.97f, 0.9f, buttonfont);
#endif
        }

        public override void Draw(GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                case GameState.Closing:
                    this._background.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._title.Draw(new Color(Math.Min(this._scale, Color.SteelBlue.R), Math.Min(this._scale, Color.SteelBlue.G), Math.Min(this._scale, Color.SteelBlue.B), 1.0f));
                    this._play.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
#if WINDOWS || SURFACE
                    this._settings.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
#else
                    this._volume.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
#endif
                    this._instructions.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));   
#if WINDOWS || WINDOWS_PHONE
                    this._highscore.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
#endif
                    break;
                case GameState.Active:
                    this._background.Draw(Color.White);
                    this._title.Draw(Color.SteelBlue);
                    this._play.Draw(Color.White);
#if WINDOWS || SURFACE
                    this._settings.Draw(Color.White);
#else
                    this._volume.Draw(Color.White);
#endif
                    this._instructions.Draw(Color.White);
#if WINDOWS || WINDOWS_PHONE
                    this._highscore.Draw(Color.White);
#endif
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                    this._scale = this._scale + 0.05f;
                    if (this._scale >= 1.0f)
                    {
                        if (ConfigurationManager.GetInstance().Volume > 0)
                        {
                            SoundManager.GetInstance(null).PlaySound("TitleScreenSoundsCompressed");
                        }
                        this._state = GameState.Active;
                    }
                    break;
                case GameState.Active:
                    if (this._play.HandleInput())
                    {
                        this._state = GameState.Closing;
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeMainGameScreen();
                        }
                    }
#if WINDOWS || SURFACE
                    else if (this._settings.HandleInput())
                    {
                        this._state = GameState.Closing;
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeSettingsScreen();
                        }
                    }
#else
                    else if (this._volume.HandleInput())
                    {
                        Texture2D volume;
                        if (ConfigurationManager.GetInstance().Volume > 0)
                        {
                            ConfigurationManager.GetInstance().SetNewSettings("en-US", 0);
                            volume = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\volumeOff");
                            SoundManager.GetInstance(null).StopSound("TitleScreenSoundsCompressed");
                        }
                        else
                        {
                            ConfigurationManager.GetInstance().SetNewSettings("", 1);
                            volume = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\volumeOn");
                            SoundManager.GetInstance(null).PlaySound("TitleScreenSoundsCompressed");
                        }
                        this._volume = new Button("", Button.Size.Custom, volume, 0.85f, 0.6f, 0.92f, 0.7f, null);
                    }
#endif
                    else if (this._instructions.HandleInput())
                    {
                        this._state = GameState.Closing;
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeFirstInstructionsScreen();
                        }
                    }
#if WINDOWS || WINDOWS_PHONE
                    else if (this._highscore.HandleInput())
                    {
                        this._state = GameState.Closing;
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeHighScoreScreen();
                        }
                    }
#endif
                    else if (this.IH.IsBackKeyPressed())
                    {
                        this._screenmanager.Exit();
                    }
                    break;
                case GameState.Closing:
                    this._scale = this._scale - 0.1f;
                    if (this._scale <= 0.0f)
                    {
                        this._state = GameState.Closed;
                    }
                    break;
            }
        } 
        #endregion
    }
}
