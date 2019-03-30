using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;

namespace WhaleRiderSim
{
    public class Whale
    {
        #region Properties
        private RectangleF _boundingbox;
        public RectangleF BoundingBox
        {
            get { return _boundingbox; }
            set { _boundingbox = value; }
        }

        private Texture _pic;
        public Texture Pic
        {
            get { return _pic; }
        }

        private SpriteBatchWrapper _sbw;
        public SpriteBatchWrapper Sbw
        {
            get { return _sbw; }
            set { _sbw = value; }
        }

        private InputHandler _ih;
        internal InputHandler IH
        {
            get { return _ih; }
            set { _ih = value; }
        }


#if WINDOWS || SURFACE
        private const float _movestep = 0.005f;
#elif WINDOWS_PHONE
        private const float _movestep = 0.008f;
#endif
        #endregion

        #region Functions
        public Whale()
        {
            this._ih = InputHandler.GetInstance();
            this._boundingbox = new RectangleF(0.0f, 0.0f, 0.0f, 0.0f);
        }

        public Whale(Texture pic, float x1, float y1, float x2, float y2)
        {
            this._pic = pic;
            this._boundingbox = new RectangleF(x1, y1, x2 - x1, y2 - y1);
            this._ih = InputHandler.GetInstance();
        }

        public virtual void Draw(Color color)
        {
            SpriteBatchWrapper.GetInstance().Begin();
            float headTop = this._boundingbox.Height * 5.0f / 20.0f;
            float headHeight = this._boundingbox.Height - headTop;
            float sriramLeft = this._boundingbox.Width * (7.0f / 15.0f);
            float sriramWidth = this._boundingbox.Width / 20.0f;
            RectangleF tailbounds = new RectangleF(this._boundingbox.Left, this._boundingbox.Top + headTop, this._boundingbox.Width, headHeight);
            RectangleF srirambounds = new RectangleF(this._boundingbox.Left + sriramLeft, this._boundingbox.Top, sriramWidth, this._boundingbox.Height);
            SpriteBatchWrapper.GetInstance().Draw(this._pic as Texture2D, this._boundingbox, color);
            SpriteBatchWrapper.GetInstance().End();
        }

        public virtual bool HandleInput()
        {
            this.IH.Update();
            Vector2 velocity = new Vector2(0.0f, 0.0f);
            if (this.IH.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.W))
            {
                velocity.Y = -1.0f;
            }
            else if (this.IH.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.S))
            {
                velocity.Y = 1.0f;
            }
            if (this.IH.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.D))
            {
                velocity.X = 1.0f;
            }
            else if (this.IH.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.A))
            {
                velocity.X = -1.0f;
            }
            if (velocity.Equals(Vector2.Zero))
            {
                if (this.IH.IsLeftMouseButtonDown())
                {
                    Vector2 mousePos = this.IH.GetMousePosition();
                    Vector2 centerPos = new Vector2(this._boundingbox.Left + this._boundingbox.Width / 2.0f, this._boundingbox.Top + this._boundingbox.Height / 2.0f);
                    if (Math.Abs(centerPos.X - mousePos.X) < 0.01f && Math.Abs(centerPos.Y - mousePos.Y) < 0.01f)
                    {
                        return true;
                    }
                    else
                    {
                        velocity = new Vector2(mousePos.X - centerPos.X, mousePos.Y - centerPos.Y);
                    }
                }
                else
                {
                    return false;
                }
            }
            velocity.Normalize();
            velocity.X *= Whale._movestep;
            velocity.Y *= Whale._movestep;    
            this._boundingbox.Location = new Vector2(this._boundingbox.Left + velocity.X, this._boundingbox.Top + velocity.Y);
            if (this._boundingbox.Top < 0.25f)
            {
                this._boundingbox.Location = new Vector2(this._boundingbox.Left, 0.25f);
            }
            if (this._boundingbox.Bottom > 1.0f)
            {
                this._boundingbox.Location = new Vector2(this._boundingbox.Left, 1.0f - this._boundingbox.Height);
            }
            if (this._boundingbox.Right > 0.75f)
            {
                this._boundingbox.Location = new Vector2(0.75f - this._boundingbox.Width, this._boundingbox.Top);
            }
            if (this._boundingbox.Left < 0.0f)
            {
                this._boundingbox.Location = new Vector2(0.0f, this._boundingbox.Top);
            }
            return true;
        }

        public Boolean Intersects(Bubble b)
        {
            float headTop = this._boundingbox.Height * 5.0f / 20.0f;
            float headHeight = this._boundingbox.Height - headTop;
            float sriramLeft = this._boundingbox.Width * (7.0f / 15.0f);
            float sriramWidth = this._boundingbox.Width / 20.0f;
            RectangleF tailbounds = new RectangleF(this._boundingbox.Left, this._boundingbox.Top + headTop, this._boundingbox.Width, headHeight);
            RectangleF srirambounds = new RectangleF(this._boundingbox.Left + sriramLeft, this._boundingbox.Top, sriramWidth, this._boundingbox.Height);
            return b.Intersects(srirambounds) || b.Intersects(tailbounds);
        }
        #endregion
    }
}