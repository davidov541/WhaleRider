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
    public class ThirdInstructionsScreen : GameScreen
    {
        #region Properties
        private Background _background;
        public Background Background
        {
            get { return _background; }
            set { _background = value; }
        }

        private Button _next;
        internal Button Next
        {
            get { return _next; }
            set { _next = value; }
        }

        private Button _back;
        internal Button Back
        {
            get { return _back; }
            set { _back = value; }
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

        private Text _instructions;
        internal Text Instructions
        {
            get { return _instructions; }
            set { _instructions = value; }
        }

        private Text _currquerylabel;
        internal Text CurrQueryLabel
        {
            get { return _currquerylabel; }
            set { _currquerylabel = value; }
        }

        private Text _currquery;
        internal Text CurrQuery
        {
            get { return _currquery; }
            set { _currquery = value; }
        }

        private Text _avoidbadqueries;
        private Text _youloosewhen;

        private InputHandler _ih;
        internal InputHandler IH
        {
            get { return _ih; }
            set { _ih = value; }
        }

        private Whale _sqlwhale;
        internal Whale SQLWhale
        {
            get { return _sqlwhale; }
            set { _sqlwhale = value; }
        }

        private List<Icon> _latedayicons;
        internal List<Icon> LateDayIcons
        {
            get { return _latedayicons; }
            set { _latedayicons = value; }
        }
        #endregion

        #region Functions
        public ThirdInstructionsScreen()
        {
        }

        public ThirdInstructionsScreen(ScreenManager sm)
            : base(sm)
        {
            Texture2D button = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\buttonsexpanded");
            SpriteFont font = this._screenmanager.Content.Load<SpriteFont>(StringResources.Simple_font);
            SpriteFont codeFont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Code_font);
            SpriteFont headerfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.SecondInstHeader_font);
            SpriteFont buttonfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Button_font);

            this.IH = InputHandler.GetInstance();
            this._background = new Background(this._screenmanager.Content.Load<Texture2D>(@"Backgrounds\tallwater"));

            this._instructions = new Text(StringResources.KeepYourLateDays, 0.5f, 0.1f, headerfont, Text.Justification.Center);
            this._avoidbadqueries = new Text(StringResources.AvoidBadQueries, 0.22f, 0.28f, font);
            this._youloosewhen = new Text(StringResources.YouLoseWhen, 0.6f, 0.6f, font);

            this._latedayicons = new List<Icon>();
            for (int i = 0; i < 3; i++)
            {
                this._latedayicons.Add(new Icon(this._screenmanager.Content.Load<Texture2D>(@"UI Elements\LateDay"), 0.4f + 0.08f * i, 0.15f, 0.4f + 0.08f * i + 0.05f, 0.22f));
            }

            this._back = new Button(StringResources.Back, Button.Size.Large, button, 0.1f, 0.8f, 0.25f, 0.9f, buttonfont);
#if WINDOWS
            this._play = new Button(StringResources.Play, Button.Size.Large, button, 0.325f, 0.8f, 0.475f, 0.9f, buttonfont);
            this._settings = new Button(StringResources.Options, Button.Size.Large, button, 0.55f, 0.8f, 0.7f, 0.9f, buttonfont);
#else
            this._play = new Button(StringResources.Play, Button.Size.Large, button, 0.45f, 0.8f, 0.6f, 0.9f, buttonfont);
#endif
            this._next = new Button(StringResources.MainMenu, Button.Size.Large, button, 0.775f, 0.8f, 0.925f, 0.9f, buttonfont);

            this._sqlwhale = new Whale(this._screenmanager.Content.Load<Texture2D>(@"Sprites\Whale"), 0.0f, 0.4f, 0.3f, 0.7f);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Color c = new Color();
            Color darkColor = new Color();
            Color headerColor = new Color();
            switch (this._state)
            {
                case GameState.Opening:
                case GameState.Closing:
                    c = new Color(this._scale, this._scale, this._scale, 1.0f);
                    darkColor = c;
                    headerColor = new Color(Math.Min(this._scale, Color.SteelBlue.R), Math.Min(this._scale, Color.SteelBlue.G), Math.Min(this._scale, Color.SteelBlue.B), 1.0f);
                    break;
                case GameState.Active:
                    c = Color.White;
                    darkColor = Color.Black;
                    headerColor = Color.SteelBlue;
                    break;
            }
            this._background.Draw(c);
            this._next.Draw(c);
            this._back.Draw(c);

            this._play.Draw(c);
#if WINDOWS
            this._settings.Draw(c);
#endif
            this._instructions.Draw(headerColor);
            this._youloosewhen.Draw(c);
            this._avoidbadqueries.Draw(darkColor);

            foreach (Icon i in this._latedayicons)
            {
                i.Draw(c);
            }

            this._sqlwhale.Draw(c);
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
                    if (this._back.HandleInput() || this.IH.IsBackKeyPressed())
                    {
                        this._state = GameState.Closing;
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeSecondInstructionsScreen();
                        }
                    }
#if WINDOWS
                    else if (this._settings.HandleInput())
                    {
                        this._state = GameState.Closing;
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeSettingsScreen();
                        }
                    }
#endif
                    else if (this._next.HandleInput())
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
