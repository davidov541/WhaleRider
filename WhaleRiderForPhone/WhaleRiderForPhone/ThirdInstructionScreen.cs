using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WhaleRiderForPhone;

namespace WhaleRiderSim
{
    public class ThirdInstructionsScreen : GameScreen
    {
        #region Properties
        private Background _background;

        private Button _next;

        private Button _back;
        private Button _play;

        private Text _instructions;

        private Text _avoidbadqueries;
        private Text _youloosewhen;

        private InputHandler _ih;

        private Whale _sqlwhale;

        private List<Icon> _latedayicons;
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

            this._ih = InputHandler.GetInstance();
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
            this._play = new Button(StringResources.Play, Button.Size.Large, button, 0.45f, 0.8f, 0.6f, 0.9f, buttonfont);
            this._next = new Button(StringResources.MainMenu, Button.Size.Large, button, 0.775f, 0.8f, 0.925f, 0.9f, buttonfont);

            this._sqlwhale = new Whale(this._screenmanager.Content.Load<Texture2D>(@"Sprites\Whale"), 0.0f, 0.4f, 0.3f, 0.7f);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                case GameState.Closing:
                    this._background.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._next.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._back.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));

                    this._play.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._instructions.Draw(new Color(this._scale * Color.SteelBlue.R / 255.0f, this._scale * Color.SteelBlue.G / 255.0f, this._scale * Color.SteelBlue.B / 255.0f, 1.0f));
                    this._youloosewhen.Draw(Color.Black);
                    this._avoidbadqueries.Draw(Color.Black);
                    this._sqlwhale.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    foreach (Icon i in this._latedayicons)
                    {
                        i.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    }
                    if (this._state == GameState.Opening)
                    {
                        this._scale += 0.1f;
                    }
                    break;
                case GameState.Active:
                    this._background.Draw(Color.White);
                    this._next.Draw(Color.White);
                    this._back.Draw(Color.White);

                    this._play.Draw(Color.White);
                    this._instructions.Draw(Color.SteelBlue);
                    this._youloosewhen.Draw(Color.Black);
                    this._avoidbadqueries.Draw(Color.Black);
                    this._sqlwhale.Draw(Color.White);
                    foreach (Icon i in this._latedayicons)
                    {
                        i.Draw(Color.White);
                    }
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
                    if (this._back.HandleInput() || this._ih.IsBackKeyPressed())
                    {
                        this._state = GameState.Closing;
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeSecondInstructionsScreen();
                        }
                    }
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
