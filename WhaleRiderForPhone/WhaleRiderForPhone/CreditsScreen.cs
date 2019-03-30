using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WhaleRiderForPhone;

namespace WhaleRiderSim
{

    class CreditsScreen : GameScreen
    {
        private enum CurrScreen
        {
            Credits,
            Updates,
        }

        #region Properties
        private Button _ok;
        private Background _background;
        private Icon _parchment;
        private Button _left;
        private Button _right;
        private Text _creditsHeader;
        private InputHandler _ih;
        private Text _designHeader;
        private Text _designText;
        private Text _develHeader;
        private Text _develText;
        private Text _supportText;
        private Text _versionHeader;
        private Text _versionText;
        private CurrScreen _currMode;

        private Text _updateHeader;
        private Text _updateText;
        #endregion

        #region Functions
        public CreditsScreen(ScreenManager sm)
            : base(sm)
        {
            this._ih = InputHandler.GetInstance();
            Texture2D button = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\buttonsexpanded");
            SpriteFont font = this._screenmanager.Content.Load<SpriteFont>(StringResources.Simple_font);
            SpriteFont headerfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.HighScoresHeader_font);
            SpriteFont buttonfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Button_font);
            SpriteFont subheaderfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Subheader_font);

            this._background = new Background(this._screenmanager.Content.Load<Texture2D>(@"Backgrounds\titlescreenbackground"));
            this._parchment = new Icon(this._screenmanager.Content.Load<Texture2D>(@"Backgrounds\ParchmentBackdrop"), 0.1f, 0.05f, 0.9f, 0.8f);
            this._ok = new Button(StringResources.OK, Button.Size.Large, button, 0.4f, 0.85f, 0.6f, 0.95f, buttonfont);
            this._left = new Button(this._screenmanager.Content.Load<Texture2D>(@"UI Elements\leftarrow"), new RectangleF(0.2f, 0.15f, 0.1f, 0.05f));
            this._right = new Button(this._screenmanager.Content.Load<Texture2D>(@"UI Elements\rightarrow"), new RectangleF(0.7f, 0.15f, 0.1f, 0.05f));
            this._creditsHeader = new Text(StringResources.Credits, 0.5f, 0.2f, headerfont, Text.Justification.Center);
            this._designHeader = new Text(StringResources.GameDesignHeader + ":", 0.2f, 0.3f, font);
            this._designText = new Text(StringResources.GameDesignText, 0.55f, 0.3f, font);
            this._develHeader = new Text(StringResources.GameDevelHeader + ":", 0.2f, 0.45f, font);
            this._develText = new Text(StringResources.GameDevelText, 0.55f, 0.45f, font);
            this._supportText = new Text(StringResources.SupportText, 0.5f, 0.7f, font, Text.Justification.Center);
            this._versionHeader = new Text(StringResources.Version + ":", 0.2f, 0.55f, font);
            this._versionText = new Text("1.1", 0.55f, 0.55f, font);
            this._currMode = CurrScreen.Credits;

            this._updateHeader = new Text(StringResources.UpdateLogHeader, 0.5f, 0.2f, headerfont, Text.Justification.Center);
            this._updateText = new Text(StringResources.UpdateLog, 0.5f, 0.5f, font, Text.Justification.Center);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                case GameState.Closing:
                    this._background.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._parchment.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._left.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._right.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._ok.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    if (this._currMode == CurrScreen.Credits)
                    {
                        this._creditsHeader.Draw(Color.Black);
                        this._designHeader.Draw(Color.Black);
                        this._designText.Draw(Color.Black);
                        this._develHeader.Draw(Color.Black);
                        this._develText.Draw(Color.Black);
                        this._supportText.Draw(Color.Black);
                        this._versionText.Draw(Color.Black);
                        this._versionHeader.Draw(Color.Black);                       
                    }
                    else
                    {
                        this._updateHeader.Draw(Color.Black);
                        this._updateText.Draw(Color.Black);
                    }
                    if (this._state == GameState.Opening)
                    {
                        this._scale += 0.1f;
                    }
                    break;
                case GameState.Active:
                case GameState.Loading:
                    this._background.Draw(Color.White);
                    this._parchment.Draw(Color.White);
                    this._left.Draw(Color.White);
                    this._right.Draw(Color.White);
                    this._ok.Draw(Color.White);
                    if(this._currMode == CurrScreen.Credits)
                    {
                        this._creditsHeader.Draw(Color.Black);
                        this._designHeader.Draw(Color.Black);
                        this._designText.Draw(Color.Black);
                        this._develHeader.Draw(Color.Black);
                        this._develText.Draw(Color.Black);
                        this._supportText.Draw(Color.Black);
                        this._versionText.Draw(Color.Black);
                        this._versionHeader.Draw(Color.Black);
                    }
                    else
	                {
                        this._updateHeader.Draw(Color.Black);
                        this._updateText.Draw(Color.Black);
                    }
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
                    else if (this._ih.IsBackKeyPressed())
                    {
                        this._screenmanager.MakeTitleScreen();
                        this._state = GameState.Closing;
                    }
                    else if (this._left.HandleInput())
                    {
                        if (this._currMode == CurrScreen.Credits)
                        {
                            this._currMode = CurrScreen.Updates;
                        }
                        else
                        {
                            this._currMode = CurrScreen.Credits;
                        }
                    }
                    else if (this._right.HandleInput())
                    {
                        if (this._currMode == CurrScreen.Credits)
                        {
                            this._currMode = CurrScreen.Updates;
                        }
                        else
                        {
                            this._currMode = CurrScreen.Credits;
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
