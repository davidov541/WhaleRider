using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#if WINDOWS_PHONE
using WhaleRiderForPhone;
#endif

namespace WhaleRiderSim
{
    public class FirstInstructionsScreen : GameScreen
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

        private Text _instructions;
        internal Text Instructions
        {
            get { return _instructions; }
            set { _instructions = value; }
        }

        private InputHandler _ih;
        internal InputHandler IH
        {
            get { return _ih; }
            set { _ih = value; }
        }

#if WINDOWS
        private Button _settings;
        internal Button Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        private Icon _controlicon;
        public Icon ControlIcon { get; set; }

        private Text _uptext;
        public Text UpText { get; set; }

        private Text _downtext;
        public Text DownText { get; set; }

        private Text _lefttext;
        public Text LeftText { get; set; }

        private Text _righttext;
        public Text RightText { get; set; }
#else
        private Whale _whale;
        private Icon _finger;
        private Icon _arrow;
        private Text _movement;
#endif
        #endregion

        #region Functions
        public FirstInstructionsScreen()
        {
        }

        public FirstInstructionsScreen(ScreenManager sm)
            : base(sm)
        {
            Texture2D button = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\buttonsexpanded");
            SpriteFont font = this._screenmanager.Content.Load<SpriteFont>(StringResources.Simple_font);
            SpriteFont headerfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.FirstInstHeader_font);
            SpriteFont buttonfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Button_font);

            this.IH = InputHandler.GetInstance();
            this._background = new Background(this._screenmanager.Content.Load<Texture2D>(@"Backgrounds\tallwater"));

            this._instructions = new Text(StringResources.Controls, 0.5f, 0.1f, headerfont, Text.Justification.Center);

            this._back = new Button(StringResources.MainMenu, Button.Size.Large, button, 0.1f, 0.8f, 0.25f, 0.9f, buttonfont);
            this._next = new Button(StringResources.Next, Button.Size.Large, button, 0.775f, 0.8f, 0.925f, 0.9f, buttonfont);
#if WINDOWS
            this._play = new Button(StringResources.Play, Button.Size.Large, button, 0.325f, 0.8f, 0.475f, 0.9f, buttonfont);
            this._controlicon = new Icon(this._screenmanager.Content.Load<Texture2D>(@"Sprites\ControlsIcon"), 0.25f, 0.25f, 0.75f, 0.65f);
            this._uptext = new Text(StringResources.Up, 0.47f, 0.2f, font);
            this._downtext = new Text(StringResources.Down, 0.47f, 0.65f, font);
            this._lefttext = new Text(StringResources.Left, 0.3f, 0.65f, font);
            this._righttext = new Text(StringResources.Right, 0.65f, 0.65f, font);
            this._settings = new Button(StringResources.Options, Button.Size.Large, button, 0.55f, 0.8f, 0.7f, 0.9f, buttonfont);
#else
            this._play = new Button(StringResources.Play, Button.Size.Large, button, 0.45f, 0.8f, 0.6f, 0.9f, buttonfont);
            this._whale = new Whale(this._screenmanager.Content.Load<Texture2D>(@"Sprites\Whale"), 0.1f, 0.5f, 0.4f, 0.8f);
            this._arrow = new Icon(this._screenmanager.Content.Load<Texture2D>(@"UI Elements\MoveArrow"), 0.43f, 0.45f, 0.63f, 0.6f);
            this._finger = new Icon(this._screenmanager.Content.Load<Texture2D>(@"UI Elements\Finger"), 0.7f, 0.45f, 0.78f, 0.55f);
            this._movement = new Text(StringResources.MoveDesc, 0.1f, 0.2f, font);
#endif
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
            this._instructions.Draw(headerColor);
#if WINDOWS
            this._settings.Draw(c);
            this._controlicon.Draw(c);
            this._uptext.Draw(darkColor);
            this._downtext.Draw(c);
            this._lefttext.Draw(c);
            this._righttext.Draw(c);
#else
            this._whale.Draw(c);
            this._arrow.Draw(c);
            this._finger.Draw(c);
            this._movement.Draw(darkColor);
#endif
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
                            this._screenmanager.MakeTitleScreen();
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
                            this._screenmanager.MakeSecondInstructionsScreen();
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
