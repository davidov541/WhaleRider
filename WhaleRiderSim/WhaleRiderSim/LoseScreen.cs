using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
#if WINDOWS_PHONE
using WhaleRiderForPhone;
#endif

namespace WhaleRiderSim
{
    public class LoseScreen : GameScreen
    {
        #region Properties
        private Text _title;
        internal Text Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private Text _scoretext;
        internal Text ScoreText
        {
            get { return _scoretext; }
            set { _scoretext = value; }
        }

        private InputHandler _ih;
        internal InputHandler IH
        {
            get { return _ih; }
            set { _ih = value; }
        }

        private Button _tryagain;
        internal Button TryAgain
        {
            get { return _tryagain; }
            set { _tryagain = value; }
        }

        private Button _exit;
        internal Button Exit
        {
            get { return _exit; }
            set { _exit = value; }
        }

        private int _score;
        internal int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                SpriteFont headerfont = this._screenmanager.Content.Load<SpriteFont>(@"Fonts\HeaderFont");
                this._scoretext = new Text(StringResources.Score + ": " + this._score.ToString(), 0.5f, 0.38f, headerfont, Text.Justification.Center);
            }
        }

        private Text _loading;
        private TextBox _textbox;
        private Boolean _errormsgfinished;
        #endregion

        #region Functions
        public LoseScreen()
        {

        }

        public LoseScreen(ScreenManager sm)
            : base(sm)
        {
            Texture2D button = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\buttonsexpanded");
            SpriteFont font = this._screenmanager.Content.Load<SpriteFont>(StringResources.Simple_font);
            SpriteFont headerfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.LoseHeader_font);
            SpriteFont buttonfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Button_font);

            this._ih = InputHandler.GetInstance();
            this._title = new Text(StringResources.YouLose, 0.5f, 0.2f, headerfont, Text.Justification.Center);
            this._scoretext = new Text(StringResources.Score + ": " + this._score.ToString(), 0.5f, 0.38f, headerfont, Text.Justification.Center);
            this._tryagain = new Button(StringResources.TryAgain, Button.Size.Medium, button, 0.25f, 0.8f, 0.45f, 0.9f, buttonfont);
            this._exit = new Button(StringResources.Quit, Button.Size.Medium, button, 0.55f, 0.8f, 0.75f, 0.9f, buttonfont);
            this._textbox = TextBox.GetTextBox(this._screenmanager.Content.Load<Texture2D>(@"UI Elements\PlainSample"), new RectangleF(0.35f, 0.6f, 0.3f, 0.1f), "Enter Your Name!", font);
            this._loading = new Text(StringResources.Saving, 0.5f, 0.5f, font, Text.Justification.Center);
            this._errormsgfinished = true;
            SoundManager.GetInstance(null).PlaySound("FailCompressed");
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                case GameState.Closing:
                    this._title.Draw(new Color(Math.Min(this._scale, Color.SteelBlue.R), Math.Min(this._scale, Color.SteelBlue.G), Math.Min(this._scale, Color.SteelBlue.B), 1.0f));
                    this._scoretext.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._tryagain.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._exit.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._textbox.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    break;
                case GameState.Active:
                    this._title.Draw(Color.White);
                    this._scoretext.Draw(Color.White);
                    this._tryagain.Draw(Color.White);
                    this._exit.Draw(Color.White);
                    this._textbox.Draw(Color.Beige);
                    break;
                case GameState.Loading:
                    this._title.Draw(Color.White);
                    this._scoretext.Draw(Color.White);
                    this._tryagain.Draw(Color.White);
                    this._exit.Draw(Color.White);
                    this._textbox.Draw(Color.Beige);
                    this._loading.Draw(Color.Red);
                    break;
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                    this._scale = this._scale + 0.05f;
                    if (this._scale >= 1.0f)
                    {
                        this._state = GameState.Active;
                    }
                    break;
                case GameState.Active:
                    if (this._tryagain.HandleInput())
                    {
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeMainGameScreen();
                        }
                        this._state = GameState.Loading;
                    }
                    else if (this._exit.HandleInput() || this._ih.IsBackKeyPressed())
                    {
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeTitleScreen();
                        }
                        this._state = GameState.Loading;
                    }
                    this._textbox.HandleInput();
                    break;
                case GameState.Loading:
                    if (this._textbox.HasBeenChanged && this._errormsgfinished)
                    {
                        ConfigurationManager.GetInstance().AddNewScore(this._textbox.TextVal, this._score);
                    }
                    this._state = GameState.Closing;
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
