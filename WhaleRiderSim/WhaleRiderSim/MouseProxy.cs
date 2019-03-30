using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace WhaleRiderSim
{
    public class MouseProxy
    {
        #region Functions
        public MouseProxy()
        {
        }

        virtual public MouseState GetState()
        {
            return Mouse.GetState();
        }

        virtual public void SetPosition(int x, int y)
        {
            Mouse.SetPosition(x, y);
        } 
        #endregion
    }
}
