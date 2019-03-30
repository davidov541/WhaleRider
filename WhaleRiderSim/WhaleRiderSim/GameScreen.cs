using Microsoft.Xna.Framework;

namespace WhaleRiderSim
{
    #region Enumerations
    public enum GameState
    {
        Opening,
        Loading,
        Active,
        Pausing,
        Paused,
        Unpausing,
        Closing,
        Closed
    } 
    #endregion

    public abstract class GameScreen
    {
        #region Properties
        protected GameState _state;
        internal GameState State
        {
            get { return this._state; }
            set { _state = value; }
        }

        protected ScreenManager _screenmanager;
        internal ScreenManager ScreenManager
        {
            get { return _screenmanager; }
            set { _screenmanager = value; }
        }

        protected float _scale;
        internal float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }
        #endregion

        #region Functions
        public GameScreen()
        {
        }

        public GameScreen(ScreenManager sm)
        {
            this._screenmanager = sm;

            this._scale = 0.0f;

            this._state = GameState.Opening;
        }

        public virtual void Draw(GameTime gameTime)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        } 
        #endregion
    }
}
