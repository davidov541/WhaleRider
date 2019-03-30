using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WhaleRiderForPhone;

namespace WhaleRiderSim
{
    public class SecondInstructionsScreen : GameScreen
    {
        #region Properties
        private Background _background;

        private Button _next;

        private Button _back;
        private Button _play;

        private Text _instructions;

        private Text _dodgeinj;
        private Text _collectsql;
        private Text _trainmode;

        private InputHandler _ih;

        private Whale _sqlwhale;

        private List<Bubble> _bubbles;
        #endregion

        #region Functions
        public SecondInstructionsScreen()
        {
        }

        public SecondInstructionsScreen(ScreenManager sm)
            : base(sm)
        {
            Texture2D button = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\buttonsexpanded");
            SpriteFont font = this._screenmanager.Content.Load<SpriteFont>(StringResources.Simple_font);
            SpriteFont codeFont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Code_font);
            SpriteFont headerfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.SecondInstHeader_font);
            SpriteFont buttonfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Button_font);

            this._ih = InputHandler.GetInstance();
            this._background = new Background(this._screenmanager.Content.Load<Texture2D>(@"Backgrounds\tallwater"));

            this._instructions = new Text(StringResources.SecondInstHeader, 0.5f, 0.1f, headerfont, Text.Justification.Center);
            this._dodgeinj = new Text(StringResources.DodgeInjection, 0.65f, 0.26f, font);
            this._collectsql = new Text(StringResources.CollectSQL, 0.32f, 0.2f, font);
            this._trainmode = new Text(StringResources.TrainingModeExplain, 0.6f, 0.55f, font);

            this._back = new Button(StringResources.Back, Button.Size.Large, button, 0.1f, 0.8f, 0.25f, 0.9f, buttonfont);
            this._play = new Button(StringResources.Play, Button.Size.Large, button, 0.45f, 0.8f, 0.6f, 0.9f, buttonfont);
            this._next = new Button(StringResources.Next, Button.Size.Large, button, 0.775f, 0.8f, 0.925f, 0.9f, buttonfont);

            this._bubbles = new List<Bubble>();
            Bubble tmp_bubble = new Bubble(Vector2.Zero, QueryPartType.STAR, false);
            tmp_bubble.SetPos(new Vector2(0.5f, 0.4f));
            tmp_bubble.Update();
            this._bubbles.Add(tmp_bubble);
            tmp_bubble = new Bubble(Vector2.Zero, QueryPartType.DELETEINJECTION, false);
            tmp_bubble.SetPos(new Vector2(0.75f, 0.42f));
            tmp_bubble.Update();
            this._bubbles.Add(tmp_bubble);
            tmp_bubble = new Bubble(Vector2.Zero, QueryPartType.STAR, true);
            tmp_bubble.SetPos(new Vector2(0.5f, 0.6f));
            tmp_bubble.Update();
            this._bubbles.Add(tmp_bubble);

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
                    this._dodgeinj.Draw(Color.Black);
                    this._collectsql.Draw(Color.Black);
                    this._trainmode.Draw(Color.Black);
                    this._sqlwhale.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
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
                    this._dodgeinj.Draw(Color.Black);
                    this._collectsql.Draw(Color.Black);
                    this._trainmode.Draw(Color.Black);
                    this._sqlwhale.Draw(Color.White);
                    break;
            }

            foreach (Bubble bubble in this._bubbles)
            {
                bubble.Draw();
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                    this._scale += 0.02f;
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
                            this._screenmanager.MakeFirstInstructionsScreen();
                        }
                    }
                    else if (this._next.HandleInput())
                    {
                        this._state = GameState.Closing;
                        if (this._screenmanager != null)
                        {
                            this._screenmanager.MakeThirdInstructionsScreen();
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
