using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WhaleRiderForPhone;

namespace WhaleRiderSim
{
    public class FirstInstructionsScreen : GameScreen
    {
        #region Properties
        private Background _background;

        private Button _next;
        private Button _back;
        private Button _play;

        private Text _instructions;

        private InputHandler _ih;
        private Whale _whale;
        private Icon _finger;
        private Icon _arrow;
        private Text _movement;
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

            this._ih = InputHandler.GetInstance();
            this._background = new Background(this._screenmanager.Content.Load<Texture2D>(@"Backgrounds\tallwater"));

            this._instructions = new Text(StringResources.Controls, 0.5f, 0.1f, headerfont, Text.Justification.Center);

            this._back = new Button(StringResources.MainMenu, Button.Size.Large, button, 0.1f, 0.8f, 0.25f, 0.9f, buttonfont);
            this._next = new Button(StringResources.Next, Button.Size.Large, button, 0.775f, 0.8f, 0.925f, 0.9f, buttonfont);
            this._play = new Button(StringResources.Play, Button.Size.Large, button, 0.45f, 0.8f, 0.6f, 0.9f, buttonfont);
            this._whale = new Whale(this._screenmanager.Content.Load<Texture2D>(@"Sprites\Whale"), 0.1f, 0.5f, 0.4f, 0.8f);
            this._arrow = new Icon(this._screenmanager.Content.Load<Texture2D>(@"UI Elements\MoveArrow"), 0.43f, 0.45f, 0.63f, 0.6f);
            this._finger = new Icon(this._screenmanager.Content.Load<Texture2D>(@"UI Elements\Finger"), 0.7f, 0.45f, 0.78f, 0.55f);
            this._movement = new Text(StringResources.MoveDesc, 0.1f, 0.2f, font);
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
                    this._whale.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._arrow.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._finger.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._movement.Draw(Color.Black);
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
                    this._whale.Draw(Color.White);
                    this._arrow.Draw(Color.White);
                    this._finger.Draw(Color.White);
                    this._movement.Draw(Color.Black);
                    break;
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                    this._scale += 0.01f;
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
                            this._screenmanager.MakeTitleScreen();
                        }
                    }
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
