using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#if WINDOWS_PHONE
using WhaleRiderForPhone;
#endif

namespace WhaleRiderSim
{
    class MainGameScreen : GameScreen
    {
        #region Properties
        private Background _background;
        public Background Background
        {
            get { return _background; }
            set { _background = value; }
        }

        private Whale _whale;
        internal Whale Whale
        {
            get { return _whale; }
            set { _whale = value; }
        }

        private List<Bubble> _bubbles;
        internal List<Bubble> Bubbles
        {
            get { return _bubbles; }
            set { _bubbles = value; }
        }

        private BubbleFactory _bfactory;
        internal BubbleFactory Bfactory
        {
            get { return _bfactory; }
            set { _bfactory = value; }
        }

        private Parser _parse;
        internal Parser Parse
        {
            get { return _parse; }
            set { _parse = value; }
        }

        private Text _headertext;
        internal Text Headertext
        {
            get { return _headertext; }
            set { _headertext = value; }
        }

        private Text _currquerylabel;
        internal Text CurrQueryLabel
        {
            get { return _currquerylabel; }
            set { _currquerylabel = value; }
        }

        private Text _currquery;
        internal Text Currquery
        {
            get { return _currquery; }
            set { _currquery = value; }
        }

        private Text _scoretext;
        internal Text ScoreText
        {
            get { return _scoretext; }
            set { _scoretext = value; }
        }

        private List<Icon> _latedayicons;
        internal List<Icon> LateDayIcons
        {
            get { return _latedayicons; }
            set { _latedayicons = value; }
        }

        private int _currscore;
        internal int Currscore
        {
            get { return _currscore; }
            set { _currscore = value; }
        }

        private int _numlatedays;
        internal int Numlatedays
        {
            get { return _numlatedays; }
            set { _numlatedays = value; }
        }

        private InputHandler _ih;
        internal InputHandler IH
        {
            get { return _ih; }
            set { _ih = value; }
        } 
        #endregion

        #region Functions
        public MainGameScreen()
        {
            this._numlatedays = 3;
            this._bubbles = new List<Bubble>();
            this._latedayicons = new List<Icon>();
            for (int i = 0; i < this._numlatedays; i++)
            {
                this._latedayicons.Add(null);
            }
        }

        public MainGameScreen(ScreenManager sm)
            : base(sm)
        {
            this._numlatedays = 3;
            this.IH = InputHandler.GetInstance();
            SpriteFont headerfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.TitleHeader_font);
            SpriteFont font = this._screenmanager.Content.Load<SpriteFont>(StringResources.Simple_font);
            SpriteFont codefont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Code_font);
            this._background = new Background(this._screenmanager.Content.Load<Texture2D>(@"Backgrounds\tallwater"));

            this._latedayicons = new List<Icon>();
            for (int i = 0; i < this._numlatedays; i++)
            {
                this._latedayicons.Add(new Icon(this._screenmanager.Content.Load<Texture2D>(@"UI Elements\LateDay"), 0.5f + 0.08f * i, 0.1f, 0.5f + 0.08f * i + 0.05f, 0.17f));
            }

            this._currquery = new Text("_", 0.05f, 0.2f, codefont);
            this._currquerylabel = new Text(StringResources.CurrQuery, 0.04f, 0.1f, font);
            this._scoretext = new Text(StringResources.Score + ": " + this._currscore.ToString(), 0.78f, 0.1f, font);


            this._parse = new Parser();
            this._bubbles = new List<Bubble>();
            this._bfactory = new BubbleFactory();
            this._bfactory.addParser(this._parse);

            this._whale = new Whale(this._screenmanager.Content.Load<Texture2D>(@"Sprites\Whale"), 0.0f, 0.3667f, 0.13333f, 0.48667f);
        }

        public override void Draw(GameTime gameTime)
        {
            Color c = new Color();
            Color darkColor = new Color();
            switch (this._state)
            {
                case GameState.Opening:
                case GameState.Closing:
                case GameState.Pausing:
                case GameState.Unpausing:
                case GameState.Paused:
                    c = new Color(this._scale, this._scale, this._scale, 1.0f);
                    darkColor = c;
                    break;
                case GameState.Active:
                    c = Color.White;
                    darkColor = Color.Black;
                    break;
            }
            this._background.Draw(c);
            this._currquerylabel.Draw(darkColor);
            this._currquery.Draw(darkColor);
            this._scoretext.Draw(darkColor);
            foreach (Bubble b in this._bubbles)
            {
                b.Draw();
            }
            foreach (Icon i in this._latedayicons)
            {
                i.Draw(c);
            }
            this._whale.Draw(c);
            DebugLabel.GetInstance(null).Draw(Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                case GameState.Unpausing:
                    this._scale = this._scale + 0.05f;
                    if (this._scale >= 1.0f)
                    {
                        this._state = GameState.Active;
                    }
                    break;
                case GameState.Active:
                    this._whale.HandleInput();
                    this._bfactory.Update(gameTime);
                    if (this._bfactory.ShouldMakeBubble)
                    {
                        this._bubbles.Add(this._bfactory.MakeBubble());
                    }
                    List<Bubble> removables = new List<Bubble>();
                    foreach (Bubble b in this._bubbles)
                    {
                        b.Update();
                        if (b.CenterPos.X < 0.0f)
                        {
                            removables.Add(b);
                        }
                        if (this._whale.Intersects(b))
                        {
                            SoundManager.GetInstance(null).PlaySound("OmNomNomCompressed");
                            bool isValid = true;
                            this._currscore += this._parse.AddQuery(b.Pop(), out isValid);
                            this._currquery = new Text(this._parse.GetQueryString() + " _", this._currquery.Position.X, this._currquery.Position.Y, this._currquery.Sf);
                            this._scoretext = new Text(StringResources.Score + ": " + this._currscore.ToString(), this._scoretext.Position.X, this._scoretext.Position.Y, this._scoretext.Sf);
                            if (!isValid)
                            {
                                this._numlatedays--;
                                if (this._numlatedays < 0)
                                {
                                    if (this._screenmanager != null)
                                    {
                                        this._screenmanager.MakeLoseScreen(this._currscore);
                                    }
                                    this._state = GameState.Pausing;
                                }
                                else
                                {
                                    this._latedayicons.RemoveAt(this._latedayicons.Count - 1);
                                }
                            }
                            removables.Add(b);
                        }
                    }
                    foreach (Bubble b in removables)
                    {
                        this._bubbles.Remove(b);
                    }
                    if (this._ih.IsBackKeyPressed())
                    {
                        this.State = GameState.Pausing;
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakePausedScreen();
                        }
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
