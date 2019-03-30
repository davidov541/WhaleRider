using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WhaleRiderSim
{
    public class Icon
    {
        #region Properties
        private Texture _pic;
        public Texture Pic
        {
            get { return _pic; }
        }

        private RectangleF _boundingbox;

        public RectangleF BoundingBox
        {
            get { return _boundingbox; }
            set { _boundingbox = value; }
        }
        #endregion

        #region Functions
        public Icon()
        {
            this._boundingbox = new RectangleF(0.0f, 0.0f, 0.0f, 0.0f);
        }

        public Icon(Texture pic, float left, float top, float right, float bottom)
        {
            this._pic = pic;
            this._boundingbox = new RectangleF(left, top, right - left, bottom - top);
        }

        public void Draw(Color color)
        {
            SpriteBatchWrapper.GetInstance().Begin();
            SpriteBatchWrapper.GetInstance().Draw(this._pic as Texture2D, this._boundingbox, color);
            SpriteBatchWrapper.GetInstance().End();
        } 
        #endregion
    }
}
