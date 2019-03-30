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
using WhaleRiderForPhone;

namespace WhaleRiderSim
{
    class TitleScreen : GameScreen
    {
        #region Properties
        private Background _background;

        private Text _title;
        private Text _intraining;

        private Button _play;
        private Button _highscore;
        private Button _instructions;
        private Button _credits;
        private Button _volume;
        private Button _trainmode;

        private InputHandler _ih;
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
            SpriteFont subHeaderFont = this._screenmanager.Content.Load<SpriteFont>(@"Fonts\LongHeaderFont");
            SpriteFont buttonfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Button_font);

            this._ih = InputHandler.GetInstance();
            this._background = new Background(this._screenmanager.Content.Load<Texture2D>(@"Backgrounds\titlescreenbackground"));

            this._title = new Text(StringResources.Title_Screen, 0.5f, 0.1f, headerfont, Text.Justification.Center);
            this._intraining = new Text(StringResources.In_Training, 0.75f, 0.19f, headerfont, Text.Justification.Center);
            this._play = new Button(StringResources.Start, Button.Size.Medium, button, 0.05f, 0.8f, 0.25f, 0.9f, buttonfont);
            this._instructions = new Button(StringResources.Instructions, Button.Size.Medium, button, 0.28f, 0.8f, 0.48f, 0.9f, buttonfont);
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
            Texture2D training;
            if (ConfigurationManager.GetInstance().Train)
            {
                training = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\trainMode");
            }
            else
            {
                training = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\noTrainMode");
            }
            this._trainmode = new Button("", Button.Size.Custom, training, 0.73f, 0.6f, 0.8f, 0.7f, buttonfont);
            this._highscore = new Button(StringResources.HighScore, Button.Size.Medium, button, 0.52f, 0.8f, 0.72f, 0.9f, buttonfont);
            this._credits = new Button(StringResources.Credits, Button.Size.Medium, button, 0.75f, 0.8f, 0.95f, 0.9f, buttonfont);
        }

        public override void Draw(GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                case GameState.Closing:
                case GameState.Paused:
                case GameState.Pausing:
                case GameState.Unpausing:
                    this._background.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._title.Draw(new Color(this._scale * Color.SteelBlue.R / 255.0f, this._scale * Color.SteelBlue.G / 255.0f, this._scale * Color.SteelBlue.B / 255.0f, 1.0f));
                    this._play.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));   
                    this._volume.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._trainmode.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._instructions.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));   
                    this._play.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._highscore.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._credits.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    if (ConfigurationManager.GetInstance().Train)
                    {
                        this._intraining.Draw(new Color(this._scale, 0, 0, 1.0f));
                    }
                    if (this._state == GameState.Opening)
                    {
                        this._scale += 0.1f;
                    }
                    break;
                case GameState.Loading:
                case GameState.Active:
                    this._background.Draw(Color.White);
                    this._title.Draw(Color.SteelBlue);
                    this._play.Draw(Color.White);
                    this._trainmode.Draw(Color.White);
                    this._volume.Draw(Color.White);
                    this._instructions.Draw(Color.White);
                    this._highscore.Draw(Color.White);
                    this._credits.Draw(Color.White);
                    if (ConfigurationManager.GetInstance().Train)
                    {
                        this._intraining.Draw(Color.Red);
                    }
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                case GameState.Unpausing:
                    this._scale += 0.05f;
                    if (this._scale >= 1.0f)
                    {
                        this._state = GameState.Loading;
                    }
                    break;
                case GameState.Loading:
                    if (ConfigurationManager.GetInstance().Volume > 0)
                    {
                        SoundManager.GetInstance(null).PlaySound("TitleScreenSoundsCompressed");
                    }
                    this._state = GameState.Active;
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
                    else if (this._volume.HandleInput())
                    {
                        Texture2D volume;
                        if (ConfigurationManager.GetInstance().Volume > 0)
                        {
                            ConfigurationManager.GetInstance().SetNewSettings("en-US", 0, ConfigurationManager.GetInstance().Train);
                            volume = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\volumeOff");
                            SoundManager.GetInstance(null).StopSound("TitleScreenSoundsCompressed");
                        }
                        else
                        {
                            ConfigurationManager.GetInstance().SetNewSettings("", 1, ConfigurationManager.GetInstance().Train);
                            volume = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\volumeOn");
                            SoundManager.GetInstance(null).PlaySound("TitleScreenSoundsCompressed");
                        }
                        this._volume = new Button("", Button.Size.Custom, volume, 0.85f, 0.6f, 0.92f, 0.7f, null);
                    }
                    else if (this._trainmode.HandleInput())
                    {
                        Texture2D trainmode;
                        if (ConfigurationManager.GetInstance().Train)
                        {
                            ConfigurationManager.GetInstance().SetNewSettings("en-US", ConfigurationManager.GetInstance().Volume, false);
                            trainmode = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\noTrainMode");
                        }
                        else
                        {
                            ConfigurationManager.GetInstance().SetNewSettings("", ConfigurationManager.GetInstance().Volume, true);
                            trainmode = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\trainMode");
                            if (!ConfigurationManager.GetInstance().HasShownTrainMode)
                            {
                                this.State = GameState.Pausing;
                                this._screenmanager.MakeAboutTrainingScreen();
                            }
                        }
                        this._trainmode = new Button("", Button.Size.Custom, trainmode, 0.73f, 0.6f, 0.8f, 0.7f, null);
                    }
                    else if (this._instructions.HandleInput())
                    {
                        this._state = GameState.Closing;
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeFirstInstructionsScreen();
                        }
                    }
                    else if (this._highscore.HandleInput())
                    {
                        this._state = GameState.Closing;
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeHighScoreScreen();
                        }
                    }
                    else if (this._credits.HandleInput())
                    {
                        this._state = GameState.Closing;
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeCreditsScreen();
                        }
                    }
                    else if (this._ih.IsBackKeyPressed())
                    {
                        this._screenmanager.Exit();
                    }
                    break;
                case GameState.Pausing:
                    this._scale = this._scale - 0.1f;
                    if (this._scale <= 0.3f)
                    {
                        this._state = GameState.Paused;
                    }
                    break;
                case GameState.Paused:
                    break;
                case GameState.Closing:
                    this._scale -= 0.1f;
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
