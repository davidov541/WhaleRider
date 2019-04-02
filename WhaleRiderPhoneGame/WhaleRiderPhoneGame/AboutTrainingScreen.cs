using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhaleRiderSim;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WhaleRiderForPhone
{
    class AboutTrainingScreen : GameScreen
    {
        #region Properties
        private Button _ok;
        private Icon _parchment;
        private Text _header;
        private Text _explain;
        private InputHandler _ih;
        private List<Bubble> _bubbles;
        #endregion

        #region Functions
        public AboutTrainingScreen(ScreenManager sm)
            : base(sm)
        {
            this._ih = InputHandler.GetInstance();
            Texture2D button = this._screenmanager.Content.Load<Texture2D>(@"UI Elements\buttonsexpanded");
            SpriteFont font = this._screenmanager.Content.Load<SpriteFont>(StringResources.Simple_font);
            SpriteFont headerfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.HighScoresHeader_font);
            SpriteFont buttonfont = this._screenmanager.Content.Load<SpriteFont>(StringResources.Button_font);

            this._header = new Text(StringResources.TrainingModeHeader, 0.5f, 0.2f, headerfont, Text.Justification.Center);
            this._parchment = new Icon(this._screenmanager.Content.Load<Texture2D>(@"Backgrounds\ParchmentBackdrop"), 0.1f, 0.05f, 0.9f, 0.8f);
            this._explain = new Text(StringResources.TrainingModeExplain, 0.2f, 0.3f, font);
            this._ok = new Button(StringResources.OK, Button.Size.Large, button, 0.4f, 0.65f, 0.6f, 0.75f, buttonfont);


            this._bubbles = new List<Bubble>();
            Bubble tmp_bubble = new Bubble(Vector2.Zero, QueryPartType.SELECT, true);
            tmp_bubble.SetPos(new Vector2(0.75f, 0.35f));
            tmp_bubble.Update();
            this._bubbles.Add(tmp_bubble);
            
            tmp_bubble = new Bubble(Vector2.Zero, QueryPartType.FROM, false);
            tmp_bubble.SetPos(new Vector2(0.75f, 0.55f));
            tmp_bubble.Update();
            this._bubbles.Add(tmp_bubble);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (this._state)
            {
                case GameState.Opening:
                case GameState.Closing:
                    this._parchment.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    this._ok.Draw(new Color(this._scale, this._scale, this._scale, 1.0f));
                    if (this._state == GameState.Opening)
                    {
                        this._scale += 0.1f;
                    }
                    break;
                case GameState.Active:
                case GameState.Loading:
                    this._parchment.Draw(Color.White);
                    this._ok.Draw(Color.White);
                    break;
            }
            this._header.Draw(Color.Black);
            this._explain.Draw(Color.Black);
            foreach (Bubble bubble in this._bubbles)
            {
                bubble.Draw();
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
                    if (this._ok.HandleInput() || this._ih.IsBackKeyPressed())
                    {
                        this._screenmanager.UnpauseScreens();
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
