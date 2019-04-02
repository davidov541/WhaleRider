using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhaleRiderSim
{
    public class Text
    {
        #region Enumerations
        public enum Justification
        {
            Left,
            Center
        } 
        #endregion

        #region Properties
        protected String _content;
        public String Content
        {
            get { return _content; }
            set { _content = value; }
        }

        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private SpriteFont _sf;
        public SpriteFont Sf
        {
            get { return _sf; }
            set { _sf = value; }
        }

        private Justification _justification;
        private SpriteBatchWrapper _sbw; 
        #endregion

        #region Functions
        /// <summary>
        /// Default constructor. Makes a new text object at the origin with an empty string.
        /// </summary>
        public Text()
        {
            this._sbw = SpriteBatchWrapper.GetInstance();
            this._content = "";
            this.Position = new Vector2(0f, 0f);
        }

        public Text(String content, float x1, float y1, SpriteFont sf)
        {
            this._sbw = SpriteBatchWrapper.GetInstance();
            this._content = content;
            this._position = new Vector2(x1, y1);
            this._sf = sf;
            this._justification = Text.Justification.Left;
        }

        public Text(String content, float x1, float y1, SpriteFont sf, Text.Justification just)
        {
            this._sbw = SpriteBatchWrapper.GetInstance();
            this._content = content;
            this._position = new Vector2(x1, y1);
            this._sf = sf;
            this._justification = just;
        }

        public void Draw(Color color)
        {
            this._sbw.Begin();
            switch (this._justification)
            {
                case Justification.Center:
                    this._sbw.DrawStringCentered(this._sf, this._content, this.Position, color);
                    break;
                case Justification.Left:
                    this._sbw.DrawString(this._sf, this._content, this.Position, color);
                    break;
                default:
                    break;

            }
            this._sbw.End();
        }

        public bool HandleInput()
        {
            return false;
        } 
        #endregion
    }
}

