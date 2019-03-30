using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Globalization;
using WhaleRiderForPhone;

namespace WhaleRiderSim
{
    class SettingsScreen : GameScreen
    {
        #region Properties
        private Background _background;

        private Text _header;
        private Text _voltext;
        private Text _languagetext;

        private Button _volup;
        private Button _voldown;

        private Button _ok;
        private Button _cancel;
        private Button _credits;

        private Button _leftlanguage;
        private Button _rightlanguage;
        private String _currlanguage;

        private String _oldlanguage;

        private float _currvol;

        private float _oldvol;

        private InputHandler _ih;

        private const float _volumex = 0.36f;
        private const float _volumey = 0.26f;
        private const float _languagex = 0.44f;
        private const float _languagey = 0.46f;
        private const float _languageleftx = 0.36f;
        private const float _languagelefty = 0.46f;
        private const float _languagerightx = 0.57f;
        private const float _languagerighty = 0.46f;

        private List<String> _supportedLanguages; 
        #endregion

        #region Functions
        public SettingsScreen()
        {
            _supportedLanguages = new List<string>();
            _supportedLanguages.Add("en-US");
            _supportedLanguages.Add("de-DE");
            _supportedLanguages.Add("ja-JP");
        }

        public SettingsScreen(ScreenManager sm)
            : base(sm)
        {
            _supportedLanguages = new List<string>();
            _supportedLanguages.Add("en-US");
            _supportedLanguages.Add("de-DE");
            _supportedLanguages.Add("ja-JP");
            Texture2D button = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\buttonsexpanded");
            SpriteFont font = this._screenmanager.Content.Load<SpriteFont>(StringResources.Simple_font);
            SpriteFont buttonfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Button_font);
            SpriteFont headerfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.SettingsHeader_font);

            this._ih = InputHandler.GetInstance();
            this._background = new Background(this._screenmanager.Content.Load<Texture2D>(@"Backgrounds\titlescreenbackground"));
            this._header = new Text(StringResources.Options, 0.5f, 0.1f, headerfont, Text.Justification.Center);

            this._currvol = ConfigurationManager.GetInstance().Volume;
            this._oldvol = ConfigurationManager.GetInstance().Volume;
            this._voltext = new Text(StringResources.Volume + ": " + this._currvol.ToString(), _volumex, _volumey, font);

            this._volup = new Button("+", Button.Size.Small, button, 0.57f, 0.25f, 0.60f, 0.30f, buttonfont);
            this._voldown = new Button("-", Button.Size.Small, button, 0.53f, 0.25f, 0.56f, 0.30f, buttonfont);

            this._currlanguage = ConfigurationManager.GetInstance().Locale.Name;
            CultureInfo ci = new CultureInfo(this._currlanguage);
            this._oldlanguage = ConfigurationManager.GetInstance().Locale.Name;
            this._languagetext = new Text(ci.NativeName.Split('(')[0], _languagex, _languagey, font);
            this._leftlanguage = new Button("<-", Button.Size.Small, button, _languageleftx, _languagelefty, _languageleftx + 0.03f, _languagelefty + 0.05f, buttonfont);
            this._rightlanguage = new Button("->", Button.Size.Small, button, _languagerightx, _languagerighty, _languagerightx + 0.03f, _languagerighty + 0.05f, buttonfont);

            this._ok = new Button(StringResources.OK, Button.Size.Medium, button, 0.1f, 0.80f, 0.3f, 0.90f, buttonfont);
            this._credits = new Button(StringResources.Credits, Button.Size.Medium, button, 0.4f, 0.80f, 0.6f, 0.9f, buttonfont);
            this._cancel = new Button(StringResources.Cancel, Button.Size.Medium, button, 0.7f, 0.80f, 0.9f, 0.90f, buttonfont);
        }

        public override void Draw(GameTime gameTime)
        {
            Color c = new Color();
            Color headerColor = new Color();
            switch (this._state)
            {
                case GameState.Opening:
                case GameState.Closing:
                    c = new Color(this._scale, this._scale, this._scale, 1.0f);
                    headerColor = new Color(this._scale * Color.SteelBlue.R / 255.0f, this._scale * Color.SteelBlue.G / 255.0f, this._scale * Color.SteelBlue.B / 255.0f, 1.0f);
                    break;
                case GameState.Active:
                    c = Color.White;
                    headerColor = Color.SteelBlue;
                    break;
            }
            this._background.Draw(c);
            this._header.Draw(headerColor);
            this._voltext.Draw(Color.Black);
            this._volup.Draw(c);
            this._voldown.Draw(c);
            this._ok.Draw(c);
            this._cancel.Draw(c);
            this._credits.Draw(c);

            this._languagetext.Draw(Color.Black);
            this._leftlanguage.Draw(c);
            this._rightlanguage.Draw(c);

        }

        public override void Update(GameTime gameTime)
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
                    if (this._volup.HandleInput())
                    {
                        if (!(this._currvol > 9.0f))
                        {
                            this._currvol = this._currvol + 1.0f;
                            if (this._screenmanager != null)
                            {
                                this._voltext = new Text(StringResources.Volume + ": " + this._currvol.ToString(), _volumex, _volumey, this._screenmanager.Content.Load<SpriteFont>(@"Fonts\SimpleFont"));
                            }
                        }
                    }
                    else if (this._voldown.HandleInput())
                    {
                        if (!(this._currvol < 1.0f))
                        {
                            this._currvol = this._currvol - 1.0f;
                            if (this._screenmanager != null)
                            {
                                this._voltext = new Text(StringResources.Volume + ": " + this._currvol.ToString(), _volumex, _volumey, this._screenmanager.Content.Load<SpriteFont>(@"Fonts\SimpleFont"));
                            }
                        }
                    }
                    else if (this._ok.HandleInput())
                    {
                        this._state = GameState.Closing;

                        ConfigurationManager.GetInstance().SetNewSettings(this._currlanguage, this._currvol, false);

                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeTitleScreen();
                        }
                    }
                    else if (this._cancel.HandleInput())
                    {
                        this._state = GameState.Closing;
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeTitleScreen();
                        }
                    }
                    else if (this._credits.HandleInput())
                    {
                        this._state = GameState.Closing;
                        this._screenmanager.MakeCreditsScreen();
                    }
                    else if (this._leftlanguage.HandleInput())
                    {
                        int currIndex = 0;
                        foreach (String s in this._supportedLanguages)
                        {
                            if (s.Equals(this._currlanguage))
                            {
                                break;
                            }
                            currIndex++;
                        }
                        if (currIndex == 0)
                        {
                            this._currlanguage = _supportedLanguages[_supportedLanguages.Count - 1];
                        }
                        else
                        {
                            this._currlanguage = _supportedLanguages[currIndex - 1];
                        }
                        if (this._screenmanager != null)
                        {
                            CultureInfo ci = new CultureInfo(this._currlanguage);
                            this._languagetext = new Text(ci.NativeName.Split('(')[0], _languagex, _languagey, this._screenmanager.Content.Load<SpriteFont>(@"Fonts\SimpleFont"));
                        }
                    }
                    else if (this._rightlanguage.HandleInput())
                    {
                        int currIndex = 0;
                        foreach (String s in this._supportedLanguages)
                        {
                            if (s.Equals(this._currlanguage))
                            {
                                break;
                            }
                            currIndex++;
                        }
                        if (currIndex == _supportedLanguages.Count - 1)
                        {
                            this._currlanguage = _supportedLanguages[0];
                        }
                        else
                        {
                            this._currlanguage = _supportedLanguages[currIndex + 1];
                        }
                        if (this._screenmanager != null)
                        {
                            CultureInfo ci = new CultureInfo(this._currlanguage);
                            this._languagetext = new Text(ci.NativeName.Split('(')[0], _languagex, _languagey, this._screenmanager.Content.Load<SpriteFont>(@"Fonts\SimpleFont"));
                        }
                    }
                    else if (this._ih.IsBackKeyPressed())
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