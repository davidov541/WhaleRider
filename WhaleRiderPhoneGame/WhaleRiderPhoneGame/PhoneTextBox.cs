using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using WhaleRiderForPhone;

namespace WhaleRiderSim
{
    class PhoneTextBox : TextBox
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

        public PhoneTextBox(Texture pic, RectangleF rect, String defaulttext, SpriteFont font)
        {
            this._ih = InputHandler.GetInstance();
            this._boundingbox = rect;
            this._pic = (Texture2D)pic;
            this._textval = defaulttext;
            this._font = font;
            this._color = Color.Gray;
            this._hasbeenchanged = false;
        }

        public override void Draw(Color color)
        {
            SpriteBatchWrapper sbw = SpriteBatchWrapper.GetInstance();
            sbw.Begin();
            sbw.DrawButton(new Rectangle(0, 0, 10, 10), this._boundingbox, this._pic, this._textval, this._font, this._color, Color.White);
            sbw.End();
        }

        public override bool HandleInput()
        {
            Vector2? leftClick = this._ih.GetLeftClick();
            if (leftClick != null)
            {
                if (this._boundingbox.ContainsPoint(leftClick.Value))
                {
                    AsyncCallback acb = new AsyncCallback(TextInputFinished);
                    Guide.BeginShowKeyboardInput(PlayerIndex.One, StringResources.Name, StringResources.NameDescription, "", acb, null, false);
                    this._color = Color.Black;
                    return true;
                }
            }
            return false;
        }

        public void TextInputFinished(IAsyncResult result)
        {
            this._hasbeenchanged = true;
            this._textval = Guide.EndShowKeyboardInput(result);
        }
    }
}
