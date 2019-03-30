using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WhaleRiderForPhone;

namespace WhaleRiderSim
{
    public class PauseScreen : GameScreen
    {
        #region Properties
        private Text _title;

        private InputHandler _ih;

        private Button _resume;
        private Button _quit;
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
            this._state = GameState.Active;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                case GameState.Closing:
                    this._title.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._resume.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._quit.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    if (this._state == GameState.Opening)
                    {
                        this._scale += 0.1f;
                    }
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
                    this._scale = this._scale + 0.01f;
                    if (this._scale >= 1.0f)
                    {
                        this._state = GameState.Active;
                    }
                    break;
                case GameState.Active:
                    if (this._resume.HandleInput() || this._ih.IsBackKeyPressed())
                    {
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.UnpauseScreens();
                        }
                        this._state = GameState.Closed;
                    }
                    else if (this._quit.HandleInput())
                    {
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeTitleScreen();
                        }
                        this._state = GameState.Closed;
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
