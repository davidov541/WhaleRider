using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WhaleRiderSim
{
    class CircleF
    {
        Vector2 _center;
        private float _radiussquared;
        public CircleF(Vector2 center, float radius)
        {
            this._center = center;
            this._radiussquared = radius*radius;
        }


        public Boolean IntersectsWith(RectangleF rect)
        {
            float rectcenterx = (rect.Left + rect.Right)/2;
            float rectcentery = (rect.Bottom + rect.Top)/2;
            return Math.Pow((rectcenterx - this._center.X),2) + Math.Pow((rectcentery - this._center.Y),2) < this._radiussquared;
           
        }
    }
}
