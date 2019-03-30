using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;

namespace WhaleRiderSim
{
    abstract class TextBox
    {
        #region Properties
        abstract internal String TextVal
        {
            get;
            set;
        }

        abstract internal Boolean HasBeenChanged
        {
            get;
            set;
        }
        #endregion
        #region Functions
        public abstract void Draw(Color color);

        public abstract bool HandleInput();

        public static TextBox GetTextBox(Texture pic, RectangleF rect, String defaulttext, SpriteFont font)
        {
#if WINDOWS
            return new WindowsTextBox(pic, rect, defaulttext, font);
#elif WINDOWS_PHONE
            return new PhoneTextBox(pic, rect, defaulttext, font);
#elif SURFACE
            return new SurfaceTextBox(pic, rect, defaulttext, font);
#endif
        }
        #endregion
    }
}
