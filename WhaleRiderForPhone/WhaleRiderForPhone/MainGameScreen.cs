using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WhaleRiderForPhone;

namespace WhaleRiderSim
{
    class MainGameScreen : GameScreen
    {
        #region Properties
        private Background _background;

        private Whale _whale;

        private List<Bubble> _bubbles;

        private BubbleFactory _bfactory;

        private Parser _parse;

        private Text _currquerylabel;
        private Text _currquery;
        private Text _scoretext;

        private List<Icon> _latedayicons;

        private int _currscore;

        private int _numlatedays;

        private InputHandler _ih;
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
            this._ih = InputHandler.GetInstance();
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

            this._whale = new Whale(this._screenmanager.Content.Load<Texture2D>(@"Sprites\Whale"), 0.0f, 0.3667f, 0.13333f, 0.48667f);
        }

        public override void Draw(GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                case GameState.Closing:
                case GameState.Pausing:
                case GameState.Unpausing:
                case GameState.Paused:
                    this._background.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._currquerylabel.Draw(Color.Black);
                    this._currquery.Draw(Color.Black);
                    this._scoretext.Draw(Color.Black);
                    foreach (Icon i in this._latedayicons)
                    {
                        i.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    }
                    this._whale.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    if (this._state == GameState.Opening || this._state == GameState.Unpausing)
                    {
                        this._scale += 0.1f;
                    }
                    break;
                case GameState.Active:
                    this._background.Draw(Color.White);
                    this._currquerylabel.Draw(Color.Black);
                    this._currquery.Draw(Color.Black);
                    this._scoretext.Draw(Color.Black);
                    foreach (Icon i in this._latedayicons)
                    {
                        i.Draw(Color.White);
                    }
                    this._whale.Draw(Color.White);
                    break;
            }
            foreach (Bubble b in this._bubbles)
            {
                b.Draw();
            }
            DebugLabel.GetInstance(null).Draw(Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                case GameState.Unpausing:
                    this._scale = this._scale + 0.01f;
                    if (this._scale >= 1.0f)
                    {
                        this._state = GameState.Active;
                    }
                    break;
                case GameState.Active:
                    this._whale.HandleInput();
                    Bubble newBubble = null;
                    if ((newBubble = this._bfactory.MakeBubble(gameTime, this._parse)) != null)
                    {
                        this._bubbles.Add(newBubble);
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
                            QueryPartType[] queries = this._parse.FindValidTypes();
                            foreach (Bubble bubble in this._bubbles)
                            {
                                bubble.SetValid(queries.Contains(bubble.Query.Type) && ConfigurationManager.GetInstance().Train);
                            }
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
