using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WhaleRiderForPhone;

namespace WhaleRiderSim
{
    public class ScreenManager : DrawableGameComponent
    {
        #region Properties
        private List<GameScreen> _screens;
        public List<GameScreen> Screens
        {
            get { return _screens; }
            set { _screens = value; }
        }

        private ContentManager _content;
        public ContentManager Content
        {
            get { return _content; }
            set { _content = value; }
        }

        private List<GameScreen> _screenstoadd;
        private static ScreenManager instance; 
        #endregion

        #region Functions
        public ScreenManager(Game game)
            : base(game)
        {
            ConfigurationManager.GetInstance();
            ScreenManager.instance = this;
            this._content = game.Content;
            this._screens = new List<GameScreen>();
            this._screenstoadd = new List<GameScreen>();
#if WINDOWS_PHONE
            SoundManager.GetInstance(this).AddSound("TitleScreenSoundsCompressed", true);
            SoundManager.GetInstance(this).AddSound("OmNomNomCompressed", false);
            SoundManager.GetInstance(this).AddSound("FailCompressed", false);
            SoundManager.GetInstance(this).AddSound("SuccessCompressed", false);
            SoundManager.GetInstance(this).AddSound("BuzzerCompressed", false);
            SoundManager.GetInstance(this).AddSound("ClickCompressed", false);
#endif
            this.MakeTitleScreen();
            DebugLabel.GetInstance(this.Content.Load<SpriteFont>(StringResources.Code_font));
        }

        public static ScreenManager GetInstance()
        {
            return ScreenManager.instance;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            foreach (GameScreen gs in this._screens)
            {
                gs.Draw(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            InputHandler.GetInstance().Update();
            List<GameScreen> screenstoremove = new List<GameScreen>();
            foreach (GameScreen gs in this._screens)
            {
                if (gs.State == GameState.Closed)
                {
                    screenstoremove.Add(gs);
                }
            }
            foreach (GameScreen gs in screenstoremove)
            {
                this._screens.Remove(gs);
            }
            foreach (GameScreen gs in this._screenstoadd)
            {
                this._screens.Add(gs);
            }
            this._screenstoadd.Clear();
            foreach (GameScreen gs in this._screens)
            {
                gs.Update(gameTime);
            }
        }

        public void MakeSettingsScreen()
        {
            foreach (GameScreen gs in this._screens)
            {
                gs.State = GameState.Closing;
            }
            _screenstoadd.Add(new SettingsScreen(this));
        }

        public void MakeTitleScreen()
        {
            foreach (GameScreen gs in this._screens)
            {
                gs.State = GameState.Closing;
            }
            _screenstoadd.Add(new TitleScreen(this));
        }

        public void MakeMainGameScreen()
        {
            foreach (GameScreen gs in this._screens)
            {
                gs.State = GameState.Closing;
            }
            _screenstoadd.Add(new MainGameScreen(this));
        }

        public void MakeFirstInstructionsScreen()
        {
            foreach (GameScreen gs in this._screens)
            {
                gs.State = GameState.Closing;
            }
            _screenstoadd.Add(new FirstInstructionsScreen(this));
        }

        public void MakeSecondInstructionsScreen()
        {
            foreach (GameScreen gs in this._screens)
            {
                gs.State = GameState.Closing;
            }
            _screenstoadd.Add(new SecondInstructionsScreen(this));
        }

        public void MakeThirdInstructionsScreen()
        {
            foreach (GameScreen gs in this._screens)
            {
                gs.State = GameState.Closing;
            }
            _screenstoadd.Add(new ThirdInstructionsScreen(this));
        }

        public void MakePausedScreen()
        {
            _screenstoadd.Add(new PauseScreen(this));
        }

        public void MakeAboutTrainingScreen()
        {
            _screenstoadd.Add(new AboutTrainingScreen(this));
        }

        public void MakeLoseScreen(int score)
        {
            LoseScreen ls = new LoseScreen(this);
            ls.Score = score;
            _screenstoadd.Add(ls);
        }

        public void MakeHighScoreScreen()
        {
            foreach (GameScreen gs in this._screens)
            {
                gs.State = GameState.Closing;
            }
            _screenstoadd.Add(new HighScoreScreen(this));
        }

        public void MakeCreditsScreen()
        {
            foreach (GameScreen gs in this._screens)
            {
                gs.State = GameState.Closing;
            }
            _screenstoadd.Add(new CreditsScreen(this));
        }

        public void UnpauseScreens()
        {
            foreach (GameScreen gs in this._screens)
            {
                if (gs.GetType() == typeof(PauseScreen) ||
                    gs.GetType() == typeof(AboutTrainingScreen))
                {
                    gs.State = GameState.Closing;
                }
                else
                {
                    gs.State = GameState.Unpausing;
                }
            }
        }

        public void Exit()
        {
#if WINDOWS_PHONE
            SoundManager.GetInstance(this).RemoveSound("TitleScreenSoundsCompressed");
            SoundManager.GetInstance(this).RemoveSound("OmNomNomCompressed");
            SoundManager.GetInstance(this).RemoveSound("FailCompressed");
            SoundManager.GetInstance(this).RemoveSound("SuccessCompressed");
            SoundManager.GetInstance(this).RemoveSound("BuzzerCompressed");
            SoundManager.GetInstance(this).RemoveSound("ClickCompressed");
#endif
            this.Game.Exit();
        }
        #endregion
    }
}
