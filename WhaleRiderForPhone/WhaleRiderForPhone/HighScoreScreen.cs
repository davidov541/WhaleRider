using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WhaleRiderForPhone;

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

        private enum CurrMode
        {
            NoTrain,
            Train,
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
        private Text _trainsubheader;
        private Button _trainleft;
        private Button _trainright;
        private CurrScoresShown _currshown;
        private CurrMode _currmode;
        private Text _loading;
        #endregion

        #region Functions
        public HighScoreScreen(ScreenManager sm) : base(sm)
        {
            this._ih = InputHandler.GetInstance();
            Texture2D button = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\buttonsexpanded");
            SpriteFont font = this._screenmanager.Content.Load<SpriteFont>(StringResources.Simple_font);
            SpriteFont headerfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.HighScoresHeader_font);
            SpriteFont buttonfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Button_font);
            SpriteFont subheaderfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Subheader_font);

            this._background = new Background(this._screenmanager.Content.Load<Texture2D>(@"Backgrounds\titlescreenbackground"));
            this._parchment = new Icon(this._screenmanager.Content.Load<Texture2D>(@"Backgrounds\ParchmentBackdrop"), 0.1f, 0.05f, 0.9f, 0.8f);
            this._play = new Button(StringResources.Start, Button.Size.Large, button, 0.25f, 0.85f, 0.45f, 0.95f, buttonfont);
            this._ok = new Button(StringResources.OK, Button.Size.Large, button, 0.55f, 0.85f, 0.75f, 0.95f, buttonfont);
            this._header = new Text(StringResources.HighScore, 0.5f, 0.2f, headerfont, Text.Justification.Center);
            this._subheader = new Text(StringResources.Local, 0.5f, 0.3f, subheaderfont, Text.Justification.Center);
            this._names = new Text(" ", 0.35f, 0.42f, font, Text.Justification.Center);
            this._scores = new Text(" ", 0.58f, 0.42f, font, Text.Justification.Center);
            this._left = new Button(this._screenmanager.Content.Load<Texture2D>(@"UI Elements\leftarrow"), new RectangleF(0.2f, 0.25f, 0.1f, 0.05f));
            this._right = new Button(this._screenmanager.Content.Load<Texture2D>(@"UI Elements\rightarrow"), new RectangleF(0.7f, 0.25f, 0.1f, 0.05f));
            this._trainleft = new Button(this._screenmanager.Content.Load<Texture2D>(@"UI Elements\leftarrow"), new RectangleF(0.2f, 0.35f, 0.1f, 0.05f));
            this._trainright = new Button(this._screenmanager.Content.Load<Texture2D>(@"UI Elements\rightarrow"), new RectangleF(0.7f, 0.35f, 0.1f, 0.05f));
            if(ConfigurationManager.GetInstance().Train)
            {
                this._currmode = CurrMode.Train;
                this._trainsubheader = new Text(StringResources.Training, 0.5f, 0.4f, subheaderfont, Text.Justification.Center);
            }
            else
            {
                this._currmode = CurrMode.NoTrain;
                this._trainsubheader = new Text(StringResources.NonTraining, 0.5f, 0.4f, subheaderfont, Text.Justification.Center);
            }
            this._currshown = CurrScoresShown.Local;
            this._loading = new Text(StringResources.Loading, 0.5f, 0.5f, font, Text.Justification.Center);
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
                    this._header.Draw(Color.Black);
                    this._subheader.Draw(Color.Black);
                    this._trainsubheader.Draw(Color.Black);
                    this._names.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._left.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._right.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._trainleft.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._trainright.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    if (this._state == GameState.Opening)
                    {
                        this._loading.Draw(Color.Black);
                        this._scale += 0.1f;
                    }
                    break;
                case GameState.Loading:
                case GameState.Waiting:
                    this._background.Draw(Color.White);
                    this._parchment.Draw(Color.White);
                    this._ok.Draw(Color.White);
                    this._play.Draw(Color.White);
                    this._left.Draw(Color.White);
                    this._right.Draw(Color.White);
                    this._trainleft.Draw(Color.White);
                    this._trainright.Draw(Color.White);
                    this._header.Draw(Color.Black);
                    this._subheader.Draw(Color.Black);
                    this._trainsubheader.Draw(Color.Black);
                    this._loading.Draw(Color.Black);
                    break;
                case GameState.Active:
                    this._background.Draw(Color.White);
                    this._parchment.Draw(Color.White);
                    this._ok.Draw(Color.White);
                    this._play.Draw(Color.White);
                    this._left.Draw(Color.White);
                    this._right.Draw(Color.White);
                    this._trainleft.Draw(Color.White);
                    this._trainright.Draw(Color.White);
                    this._header.Draw(Color.Black);
                    this._subheader.Draw(Color.Black);
                    this._trainsubheader.Draw(Color.Black);
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
                    this._scale = this._scale + 0.02f;
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
                        hses = ConfigurationManager.GetInstance().GetLocalHighScores((int) this._currmode);
                        this._state = GameState.Active;
                        DisplayScores(hses);
                    }
                    else if (this._currshown == CurrScoresShown.Platform)
                    {
                        hses = ConfigurationManager.GetInstance().StartGetPlatformHighScores(del, (int) this._currmode);
                        this._state = GameState.Waiting;
                    }
                    else if (this._currshown == CurrScoresShown.Global)
                    {
                        hses = ConfigurationManager.GetInstance().StartGetGlobalHighScores(del, (int) this._currmode);
                        this._state = GameState.Waiting;
                    }
                    break;
                case GameState.Waiting:
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
                        this._names = new Text(" ", this._names.Position.X, this._names.Position.Y, this._names.Sf, Text.Justification.Center);
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
                        this._names = new Text(" ", this._names.Position.X, this._names.Position.Y, this._names.Sf, Text.Justification.Center);
                        this._scores = new Text(" ", this._scores.Position.X, this._scores.Position.Y, this._scores.Sf, Text.Justification.Center);
                    }
                    else if (this._trainleft.HandleInput() || this._trainright.HandleInput())
                    {
                        if (this._currmode == CurrMode.Train)
                        {
                            this._currmode = CurrMode.NoTrain;
                            this._trainsubheader = new Text(StringResources.NonTraining, this._trainsubheader.Position.X, this._trainsubheader.Position.Y, this._trainsubheader.Sf, Text.Justification.Center);
                        }
                        else
                        {
                            this._currmode = CurrMode.Train;
                            this._trainsubheader = new Text(StringResources.Training, this._trainsubheader.Position.X, this._trainsubheader.Position.Y, this._trainsubheader.Sf, Text.Justification.Center);
                        }
                        this._state = GameState.Loading;
                        this._names = new Text(" ", this._names.Position.X, this._names.Position.Y, this._names.Sf, Text.Justification.Center);
                        this._scores = new Text(" ", this._scores.Position.X, this._scores.Position.Y, this._scores.Sf, Text.Justification.Center);
                    }
                    break;
                case GameState.Closing:
                    this._scale = this._scale - 0.1f;
                    if (this._scale <= 0.0f)
                    {
                        this._state = GameState.Closed;
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
            if (scores == null)
            {
                this._loading.Content = StringResources.Error + " -- \n" + StringResources.CouldNotConnect;
            }
            else
            {
                this._state = GameState.Active;
                DisplayScores(scores);
            }
        }
        #endregion
    }
}
