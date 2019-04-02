using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WhaleRiderSim
{
    public class RectangleF
    {
        private Vector2 _pos;
        public Vector2 Location
        {
            get { return _pos; }
            set { _pos = value; }
        }

        private Vector2 _dim;

        public float Top
        {
            get { return this._pos.Y; }
        }
        public float Bottom
        {
            get { return this._pos.Y + this._dim.Y; }
        }
        public float Left
        {
            get { return this._pos.X; }
        }
        public float Right
        {
            get { return this._pos.X + this._dim.X; }
        }
        public float Width
        {
            get { return this._dim.X; }
        }
        public float Height
        {
            get { return this._dim.Y; }
        }

        public RectangleF()
        {

        }

        public RectangleF(float x, float y, float width, float height)
        {
            this._pos = new Vector2(x, y);
            this._dim = new Vector2(width, height);
        }

        public Boolean IntersectsWith(RectangleF rect)
        {
            Boolean returnable = false;
            if (rect.Left <= this.Left && rect.Right >= this.Left)
            {
                returnable = true;
            }
            else if (rect.Left <= this.Right && rect.Right >= this.Right)
            {
                returnable = true;
            }
            else if (rect.Left >= this.Left && rect.Right <= this.Right)
            {
                returnable = true;
            }
            else if (rect.Left <= this.Left && rect.Right >= this.Right)
            {
                returnable = true;
            }
            if (!returnable)
            {
                return false;
            }
            if (rect.Top <= this.Top && rect.Bottom >= this.Top)
            {
                return true;
            }
            else if (rect.Top <= this.Bottom && rect.Bottom >= this.Bottom)
            {
                return true;
            }
            else if (rect.Top >= this.Top && rect.Bottom <= this.Bottom)
            {
                return true;
            }
            else if (rect.Top <= this.Top && rect.Bottom >= this.Bottom)
            {
                return true;
            }
            return false;
        }

        public Boolean ContainsPoint(Vector2 v)
        {
            return this.Left <= v.X && this.Right >= v.X && this.Top <= v.Y && this.Bottom >= v.Y;
        }

        public override bool Equals(object obj)
        {
            RectangleF rf = (RectangleF)obj;
            return this._dim.Equals(rf._dim) && this._pos.Equals(rf._pos);
        }
    }
}
