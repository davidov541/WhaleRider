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
    public class PauseScreen : GameScreen
    {
        #region Properties
        private Text _title;
        internal Text Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private InputHandler _ih;
        internal InputHandler IH
        {
            get { return _ih; }
            set { _ih = value; }
        }

        private Button _resume;
        internal Button Resume
        {
            get { return _resume; }
            set { _resume = value; }
        }

        private Button _quit;
        internal Button Quit
        {
            get { return _quit; }
            set { _quit = value; }
        } 
        #endregion

        #region Functions
        public PauseScreen()
        {

        }

        public PauseScreen(ScreenManager sm)
            : base(sm)
        {
            Texture2D button = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\buttonsexpanded");
            SpriteFont font = this._screenmanager.Content.Load<SpriteFont>(StringResources.Simple_font);
            SpriteFont headerfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.PausedHeader_font);
            SpriteFont buttonfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Button_font);

            this._ih = InputHandler.GetInstance();
            this._title = new Text(StringResources.Paused, 0.5f, 0.35f, headerfont, Text.Justification.Center);
            this._resume = new Button(StringResources.Resume, Button.Size.Medium, button, 0.4f, 0.5f, 0.6f, 0.6f, buttonfont);
            this._quit = new Button(StringResources.Quit, Button.Size.Medium, button, 0.4f, 0.7f, 0.6f, 0.8f, buttonfont);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                case GameState.Closing:
                    this._title.Draw(new Color(Math.Min(this._scale, Color.SteelBlue.R), Math.Min(this._scale, Color.SteelBlue.G), Math.Min(this._scale, Color.SteelBlue.B), 1.0f));
                    this._resume.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._quit.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    break;
                case GameState.Active:
                    this._title.Draw(Color.White);
                    this._resume.Draw(Color.White);
                    this._quit.Draw(Color.White);
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
                    if (this._resume.HandleInput())
                    {
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.UnpauseScreens();
                        }
                        this._state = GameState.Closing;
                    }
                    else if (this._quit.HandleInput())
                    {
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeTitleScreen();
                        }
                        this._state = GameState.Closing;
                    }
                    else if (this.IH.IsBackKeyPressed())
                    {
                        ScreenManager.Game.Exit();
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
