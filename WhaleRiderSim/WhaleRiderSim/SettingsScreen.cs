using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Globalization;
#if WINDOWS_PHONE
using WhaleRiderForPhone;
#endif

namespace WhaleRiderSim
{
    class SettingsScreen : GameScreen
    {
        #region Properties
        private Background _background;
        public Background Background
        {
            get { return _background; }
            set { _background = value; }
        }

        private Text _header;

        private Text _voltext;
        internal Text VolText
        {
            get { return _voltext; }
            set { _voltext = value; }
        }

        private Text _languagetext;
        internal Text LanguageText
        {
            get { return _languagetext; }
            set { _languagetext = value; }
        }

        private Button _volup;
        internal Button VolUp
        {
            get { return _volup; }
            set { _volup = value; }
        }

        private Button _voldown;
        internal Button VolDown
        {
            get { return _voldown; }
            set { _voldown = value; }
        }

        private Button _ok;
        internal Button OK
        {
            get { return _ok; }
            set { _ok = value; }
        }

        private Button _cancel;
        internal Button Cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }

        private Button _leftlanguage;
        internal Button LeftLanguage
        {
            get { return _leftlanguage; }
            set { this._leftlanguage = value; }
        }

        private Button _rightlanguage;
        internal Button RightLanguage
        {
            get { return _rightlanguage; }
            set { this._rightlanguage = value; }
        }

        private String _currlanguage;
        internal String CurrLanguage
        {
            get { return _currlanguage; }
            set { _currlanguage = value; }
        }

        private String _oldlanguage;
        internal String OldLanguage
        {
            get { return _oldlanguage; }
            set { _oldlanguage = value; }
        }

        private float _currvol;
        internal float CurrVol
        {
            get { return _currvol; }
            set { _currvol = value; }
        }

        private float _oldvol;
        internal float OldVol
        {
            get { return _oldvol; }
            set { _oldvol = value; }
        }

        private InputHandler _ih;
        internal InputHandler IH
        {
            get { return _ih; }
            set { _ih = value; }
        }

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

            this.IH = InputHandler.GetInstance();
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

            this._ok = new Button(StringResources.OK, Button.Size.Medium, button, 0.55f, 0.80f, 0.75f, 0.90f, buttonfont);
            this._cancel = new Button(StringResources.Cancel, Button.Size.Medium, button, 0.25f, 0.80f, 0.45f, 0.90f, buttonfont);
        }

        public override void Draw(GameTime gameTime)
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
            this._header.Draw(headerColor);
            this._voltext.Draw(darkColor);
            this._volup.Draw(c);
            this._voldown.Draw(c);
            this._ok.Draw(c);
            this._cancel.Draw(c);

            this._languagetext.Draw(darkColor);
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

                        ConfigurationManager.GetInstance().SetNewSettings(this._currlanguage, this._currvol);

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
//#endif
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