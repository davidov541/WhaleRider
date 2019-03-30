using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace WhaleRiderSim
{
    public class DebugLabel : Text
    {
        private static DebugLabel _instance;

        private DebugLabel(SpriteFont font)
        {
            this.Position = new Microsoft.Xna.Framework.Vector2(0, 0);
            this._content = "";
            this.Sf = font;
        }

        public static DebugLabel GetInstance(SpriteFont font)
        {
            if (DebugLabel._instance == null)
            {
                DebugLabel._instance = new DebugLabel(font);
            }
            return DebugLabel._instance;
        }
    }
}
