using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;

namespace WhaleRiderSim
{
    class SurfaceTextBox : TextBox
    {
        private String _textval;
        internal override String TextVal
        {
            get { return _textval; }
            set { _textval = value; }
        }

        private Boolean _hasbeenchanged;
        internal override Boolean HasBeenChanged
        {
            get { return _hasbeenchanged; }
            set { _hasbeenchanged = value; }
        }

        private InputHandler _ih;
        private Texture2D _pic;
        private RectangleF _boundingbox;
        private SpriteFont _font;
        private Color _color;

        private readonly int _flickerrate = 100;
        private int _currflicker = -1;
        private Boolean _cursoron;
        private readonly float _cursormargin = 0.01f;
        private readonly float _cursorwidth = 0.01f;

        public SurfaceTextBox(Texture pic, RectangleF rect, String defaulttext, SpriteFont font)
        {
            this._ih = InputHandler.GetInstance();
            this._boundingbox = rect;
            this._pic = (Texture2D)pic;
            this._textval = defaulttext;
            this._font = font;
            this._color = Color.Gray;
            this._hasbeenchanged = false;
            this._cursoron = true;
        }

        public override void Draw(Color color)
        {
            SpriteBatchWrapper sbw = SpriteBatchWrapper.GetInstance();
            sbw.Begin();
            sbw.DrawButton(new Rectangle(0, 0, 10, 10), this._boundingbox, this._pic, this._textval, this._font, this._color, Color.White);
            if (this._cursoron)
            {
                if (this._hasbeenchanged)
                {
                    sbw.DrawSprite(this._pic, new Rectangle(0, 0, 10, 10), new RectangleF(this._boundingbox.Left + this._font.MeasureString(this._textval).X + this._cursormargin, this._boundingbox.Top + this._cursormargin, this._cursorwidth, this._boundingbox.Height - 2 * this._cursormargin));
                }
                else
                {
                    sbw.DrawSprite(this._pic, new Rectangle(0, 0, 10, 10), new RectangleF(this._boundingbox.Left + this._cursormargin, this._boundingbox.Top + this._cursormargin, this._cursorwidth, this._boundingbox.Height - 2 * this._cursormargin));
                }
            }
            sbw.End();
        }

        public override bool HandleInput()
        {
            if (this._currflicker == 0)
            {
                this._currflicker = this._flickerrate;
                this._cursoron = !this._cursoron;
            }
            else
            {
                this._currflicker--;
            }
            char c = this._ih.GetKeyPressed();
            if (c != '\0')
            {
                if (this._hasbeenchanged && this._textval.Length > 0)
                {
                    if (c == (char)8)
                    {
                        this._textval = this._textval.Substring(0, this._textval.Length - 1);
                    }
                    else
                    {
                        this._textval += c;
                    }
                }
                else if (c != (char) 8)
                {
                    this._hasbeenchanged = true;
                    this._textval = c.ToString();
                    this._color = Color.Black;
                }
            }
            return false;
        }
    }
}
