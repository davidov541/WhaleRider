using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
#if WINDOWS_PHONE
using WhaleRiderForPhone;
#endif

namespace WhaleRiderSim
{
    class HighScoreScreen : GameScreen
    {
        private enum CurrScoresShown
        {
            Local,
            Platform,
            Global
        }

        #region Properties
        private Button _ok;
        private Background _background;
        private Icon _parchment;
        private Text _header;
        private Text _names;
        private Text _scores;
        private Button _play;
        private InputHandler _ih;
        private Button _left;
        private Button _right;
        private Text _subheader;
        private CurrScoresShown _currshown;
        #endregion

        #region Functions
        public HighScoreScreen(ScreenManager sm) : base(sm)
        {
            this._ih = InputHandler.GetInstance();
            Texture2D button = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\buttonsexpanded");
            SpriteFont font = this._screenmanager.Content.Load<SpriteFont>(StringResources.Simple_font);
            SpriteFont headerfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.HighScoresHeader_font);
            SpriteFont buttonfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Button_font);

            this._background = new Background(this._screenmanager.Content.Load<Texture2D>(@"Backgrounds\titlescreenbackground"));
            this._parchment = new Icon(this._screenmanager.Content.Load<Texture2D>(@"Backgrounds\ParchmentBackdrop"), 0.1f, 0.05f, 0.9f, 0.8f);
            this._play = new Button(StringResources.Start, Button.Size.Large, button, 0.25f, 0.85f, 0.45f, 0.95f, buttonfont);
            this._ok = new Button(StringResources.OK, Button.Size.Large, button, 0.55f, 0.85f, 0.75f, 0.95f, buttonfont);
            this._header = new Text(StringResources.HighScore, 0.5f, 0.2f, headerfont, Text.Justification.Center);
            this._subheader = new Text(StringResources.Local, 0.5f, 0.3f, headerfont, Text.Justification.Center);
            this._names = new Text(" ", 0.35f, 0.35f, font, Text.Justification.Center);
            this._scores = new Text(StringResources.Loading, 0.58f, 0.35f, font, Text.Justification.Center);
            this._left = new Button(this._screenmanager.Content.Load<Texture2D>(@"UI Elements\leftarrow"), new RectangleF(0.2f, 0.45f, 0.1f, 0.1f));
            this._right = new Button(this._screenmanager.Content.Load<Texture2D>(@"UI Elements\rightarrow"), new RectangleF(0.7f, 0.45f, 0.1f, 0.1f));
            this._currshown = CurrScoresShown.Local;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                case GameState.Closing:
                    this._background.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._parchment.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._ok.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._play.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._header.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._subheader.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._names.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._left.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._right.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    break;
                case GameState.Active:
                case GameState.Loading:
                    this._background.Draw(Color.White);
                    this._parchment.Draw(Color.White);
                    this._ok.Draw(Color.White);
                    this._play.Draw(Color.White);
                    this._left.Draw(Color.White);
                    this._right.Draw(Color.White);
                    this._header.Draw(Color.Black);
                    this._subheader.Draw(Color.Black);
                    this._names.Draw(Color.Black);
                    this._scores.Draw(Color.Black);
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
                        this._state = GameState.Loading;
                    }
                    break;
                case GameState.Loading:
                    List<HighScore> hses = new List<HighScore>();
                    ConfigurationManager.AsyncReturn del = getHighScoresAsync;
                    if (this._currshown == CurrScoresShown.Local)
                    {
                        hses = ConfigurationManager.GetInstance().GetLocalHighScores();
                    }
                    else if (this._currshown == CurrScoresShown.Platform)
                    {
                        hses = ConfigurationManager.GetInstance().StartGetPlatformHighScores(del);
                    }
                    else if (this._currshown == CurrScoresShown.Global)
                    {
                        hses = ConfigurationManager.GetInstance().StartGetGlobalHighScores(del);
                    }
                    DisplayScores(hses);
                    this._state = GameState.Active;
                    break;
                case GameState.Active:
                    if (this._ok.HandleInput())
                    {
                        this._state = GameState.Closing;
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeTitleScreen();
                        }
                    }
                    else if (this._play.HandleInput())
                    {
                        this._state = GameState.Closing;
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeMainGameScreen();
                        }
                    }
                    else if (this._ih.IsBackKeyPressed())
                    {
                        this._screenmanager.MakeTitleScreen();
                        this._state = GameState.Closing;
                    }
                    else if (this._left.HandleInput())
                    {
                        if (this._currshown == CurrScoresShown.Global)
                        {
                            this._currshown = CurrScoresShown.Platform;
                            this._subheader = new Text(ConfigurationManager.GetInstance().PlatformName, this._subheader.Position.X, this._subheader.Position.Y, this._subheader.Sf, Text.Justification.Center);
                        }
                        else if (this._currshown == CurrScoresShown.Local)
                        {
                            this._currshown = CurrScoresShown.Global;
                            this._subheader = new Text(StringResources.Global, this._subheader.Position.X, this._subheader.Position.Y, this._subheader.Sf, Text.Justification.Center);
                        }
                        else if (this._currshown == CurrScoresShown.Platform)
                        {
                            this._currshown = CurrScoresShown.Local;
                            this._subheader = new Text(StringResources.Local, this._subheader.Position.X, this._subheader.Position.Y, this._subheader.Sf, Text.Justification.Center);
                        }
                        this._state = GameState.Loading;
                        this._names = new Text(StringResources.Loading, this._names.Position.X, this._names.Position.Y, this._names.Sf, Text.Justification.Center);
                        this._scores = new Text(" ", this._scores.Position.X, this._scores.Position.Y, this._scores.Sf, Text.Justification.Center);
                    }
                    else if (this._right.HandleInput())
                    {
                        if (this._currshown == CurrScoresShown.Local)
                        {
                            this._currshown = CurrScoresShown.Platform;
                            this._subheader = new Text(ConfigurationManager.GetInstance().PlatformName, this._subheader.Position.X, this._subheader.Position.Y, this._subheader.Sf, Text.Justification.Center);
                        }
                        else if (this._currshown == CurrScoresShown.Platform)
                        {
                            this._currshown = CurrScoresShown.Global;
                            this._subheader = new Text(StringResources.Global, this._subheader.Position.X, this._subheader.Position.Y, this._subheader.Sf, Text.Justification.Center);
                        }
                        else if (this._currshown == CurrScoresShown.Global)
                        {
                            this._currshown = CurrScoresShown.Local;
                            this._subheader = new Text(StringResources.Local, this._subheader.Position.X, this._subheader.Position.Y, this._subheader.Sf, Text.Justification.Center);
                        }
                        this._state = GameState.Loading;
                        this._names = new Text(StringResources.Loading, this._names.Position.X, this._names.Position.Y, this._names.Sf, Text.Justification.Center);
                        this._scores = new Text(" ", this._scores.Position.X, this._scores.Position.Y, this._scores.Sf, Text.Justification.Center);
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

        private void DisplayScores(List<HighScore> hses)
        {
            String outStrNames = "";
            String outStrScores = "";
            foreach (HighScore score in hses)
            {
                outStrNames += score.Name + "\n";
                outStrScores += score.Score + "\n";
            }
            this._names = new Text(outStrNames, this._names.Position.X, this._names.Position.Y, this._names.Sf);
            this._scores = new Text(outStrScores, this._scores.Position.X, this._scores.Position.Y, this._scores.Sf);
        }

        public void getHighScoresAsync(List<HighScore> scores)
        {
            DisplayScores(scores);
        }
        #endregion
    }
}
