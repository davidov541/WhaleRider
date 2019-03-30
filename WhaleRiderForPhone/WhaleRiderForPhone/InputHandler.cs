using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WhaleRiderSim
{
    public abstract class InputHandler
    {
        #region Properties
        protected MouseState _previousmousestate;

        protected KeyboardState _previouskeyboardstate;

        protected MouseState _currentmousestate;

        protected KeyboardState _currentkeyboardstate;

        protected GraphicsDevice _device;
        public GraphicsDevice Device
        {
            get { return _device; }
            set { _device = value; }
        }

        protected MouseProxy _mouse;

        protected KeyboardProxy _keyboard;

        protected long _rightclicktimer;

        protected int _width;

        protected int _height;

        protected char _lastcharpressed;
        private static InputHandler _singleton;
        #endregion

        #region Functions

        public static InputHandler GetInstance()
        {
            if (InputHandler._singleton == null)
            {
#if WINDOWS
                InputHandler._singleton = new WindowsInputHandler();
#elif WINDOWS_PHONE
                InputHandler._singleton = new PhoneInputHandler();
#elif SURFACE
                InputHandler._singleton = new SurfaceInputHandler();
#endif
            }
            return InputHandler._singleton;
        }

        /// <summary>
        /// Checks to see if a click was registered during the last update. Will return the point
        /// if and only if the the left mouse button was pressed during the previous update but
        /// is was pressed during the most recent update. Returns null otherwise.
        /// </summary>
        /// <returns>Point, the point where the mouse was released</returns>
        public abstract Vector2? GetLeftClick();

        /// <summary>
        /// Checks to see if the left mouse button was pressed during the most recent update.
        /// </summary>
        /// <returns>True if left button was pressed, false otherwise</returns>
        public abstract Boolean IsLeftMouseButtonDown();

        public abstract Boolean IsRightMouseButtonDown();

        public abstract Vector2 GetMousePosition();

        public abstract Boolean IsKeyPressed(Keys key);

        public abstract char GetKeyPressed();

        public abstract Boolean IsBackKeyPressed();

        public abstract void Update();
        #endregion
    }
}
