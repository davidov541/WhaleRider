using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhaleRiderSim
{
    public class Button
    {
        #region Enumerations
        public enum Size 
        { 
            Small, 
            Medium, 
            Large,
            Custom
        };
        
        public enum State 
        { 
            Released, 
            Depressed, 
            Hover
        };
        #endregion

        #region Properties
        public static int TextureHeight
        {
            get { return 75; }
        }

        public static int TextureWidth
        {
            get { return 270; }
        }

        private InputHandler _ih;
        internal InputHandler IH
        {
            get { return _ih; }
            set { _ih = value; }
        }

        private SpriteBatchWrapper _batch;
        public SpriteBatchWrapper Batch
        {
            get { return _batch; }
            set { _batch = value; }
        }

        private String _text;
        public String Text
        {
            get { return _text; }
        }

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

        private SpriteFont _font;
        public SpriteFont Font
        {
            get { return _font; }
        }

        public Size SizeVal { get; set; }

        public State StateVal { get; set; }

        private Color _color; 
        #endregion

        #region Functions
        public Button()
        {
            this._text = "";
            this.IH = InputHandler.GetInstance();
            this._boundingbox = new RectangleF(0.0f, 0.0f, 0.0f, 0.0f);

        }

        public Button(string text, Button.Size size, Texture pic, float x1, float y1, float x2, float y2, SpriteFont sf)
        {
            this._text = text;
            this.SizeVal = size;
            this.IH = InputHandler.GetInstance();
            this._pic = pic;
            this._boundingbox = new RectangleF(x1, y1, x2 - x1, y2 - y1);
            this._font = sf;
            this.StateVal = Button.State.Released;
            this._color = Color.Wheat;
        }

        public Button(string text, Button.Size size, Texture pic, float x1, float y1, float x2, float y2, SpriteFont sf, Color textColor)
        {
            this._text = text;
            this.SizeVal = size;
            this.IH = InputHandler.GetInstance();
            this._pic = pic;
            this._boundingbox = new RectangleF(x1, y1, x2 - x1, y2 - y1);
            this._font = sf;
            this.StateVal = Button.State.Released;
            this._color = textColor;
        }

        public Button(Texture pic, RectangleF rect)
        {
            this._boundingbox = rect;
            this._ih = InputHandler.GetInstance();
            this._pic = pic;
            this.StateVal = Button.State.Released;
            this.SizeVal = Size.Custom;
        }

        public static Rectangle getTextureBounds(Button.Size size)
        {
            int x1, y1, x2, y2;
            x1 = 0 * Button.TextureWidth;
            switch (size)
            {
                case Button.Size.Small:
                case Size.Custom:
                    y1 = 0 * Button.TextureHeight;
                    y2 = 1 * Button.TextureHeight;
                    x2 = 159;
                    break;
                case Button.Size.Medium:
                    y1 = 1 * Button.TextureHeight + 1;
                    y2 = 2 * Button.TextureHeight + 1;
                    x2 = 210;
                    break;
                case Button.Size.Large:
                    y1 = 2 * Button.TextureHeight + 2;
                    y2 = 3 * Button.TextureHeight + 2;
                    x2 = 270;
                    break;
                default:
                    //defaults to large button
                    y1 = 2 * Button.TextureHeight + 2;
                    y2 = 3 * Button.TextureHeight + 2;
                    x2 = 270;
                    break;
            }



            return new Rectangle(x1, y1, (x2 - x1), (y2 - y1));
        }

        public virtual void Draw(Color color)
        {
            SpriteBatchWrapper sbw = SpriteBatchWrapper.GetInstance();
            sbw.Begin();
            if (this.SizeVal == Size.Custom)
            {
                sbw.Draw(this._pic as Texture2D, this._boundingbox, color);
            }
            else
            {
                sbw.DrawButton(Button.getTextureBounds(this.SizeVal), this._boundingbox, this._pic as Texture2D, this._text, this._font, this._color);
            }
            sbw.End();
        }

        /// <summary>
        /// Not entirely sure what this is supposed to do yet.
        /// </summary>
        /// <returns></returns>
        public virtual bool HandleInput()
        {
            Vector2? leftClick = this.IH.GetLeftClick();
            if (leftClick != null)
            {
                if (this._boundingbox.ContainsPoint(leftClick.Value))
                {
                    SoundManager.GetInstance(null).PlaySound("ClickCompressed");
                    return true;
                }
            }
            return false;
        }
        #endregion
    } 
}
