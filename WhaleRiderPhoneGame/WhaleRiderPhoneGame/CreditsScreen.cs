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
        #region Properties
        private Button _ok;
        private Background _background;
        private Icon _parchment;
        private Text _header;
        private InputHandler _ih;
        private Text _designHeader;
        private Text _designText;
        private Text _develHeader;
        private Text _develText;
        private Text _supportText;
        private Text _versionHeader;
        private Text _versionText;
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
            this._header = new Text(StringResources.Credits, 0.5f, 0.2f, headerfont, Text.Justification.Center);
            this._designHeader = new Text(StringResources.GameDesignHeader + ":", 0.2f, 0.3f, font);
            this._designText = new Text(StringResources.GameDesignText, 0.55f, 0.3f, font);
            this._develHeader = new Text(StringResources.GameDevelHeader + ":", 0.2f, 0.45f, font);
            this._develText = new Text(StringResources.GameDevelText, 0.55f, 0.45f, font);
            this._supportText = new Text(StringResources.SupportText, 0.5f, 0.7f, font, Text.Justification.Center);
            this._versionHeader = new Text(StringResources.Version + ":", 0.2f, 0.55f, font);
            this._versionText = new Text("1.1", 0.55f, 0.55f, font);
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
                    this._header.Draw(Color.Black);
                    this._designHeader.Draw(Color.Black);
                    this._designText.Draw(Color.Black);
                    this._develHeader.Draw(Color.Black);
                    this._develText.Draw(Color.Black);
                    this._supportText.Draw(Color.Black);
                    this._versionText.Draw(Color.Black);
                    this._versionHeader.Draw(Color.Black);
                    if (this._state == GameState.Opening)
                    {
                        this._scale += 0.1f;
                    }
                    break;
                case GameState.Active:
                case GameState.Loading:
                    this._background.Draw(Color.White);
                    this._parchment.Draw(Color.White);
                    this._ok.Draw(Color.White);
                    this._header.Draw(Color.Black);
                    this._designHeader.Draw(Color.Black);
                    this._designText.Draw(Color.Black);
                    this._develHeader.Draw(Color.Black);
                    this._develText.Draw(Color.Black);
                    this._supportText.Draw(Color.Black);
                    this._versionText.Draw(Color.Black);
                    this._versionHeader.Draw(Color.Black);
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
