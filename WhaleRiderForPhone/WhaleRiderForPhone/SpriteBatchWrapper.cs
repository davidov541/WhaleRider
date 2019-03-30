using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WhaleRiderSim
{
    public class SpriteBatchWrapper : IDisposable
    {
        #region Properties
        private static SpriteBatchWrapper _instance;
        private SpriteBatch _batch;
        private ScreenManager _sm;
        //Should be moved to maingamescreen
        private Texture2D _bubblesprite;
        private SpriteFont _bubblefont;
        private readonly float _nativewidth = 800.0f;
        private readonly float _nativeheight = 480.0f;
        private readonly float _maxwidth = 2000.0f;
        private readonly float _maxheight = 1600.0f;

        private GraphicsDevice _device;
        public GraphicsDevice Device
        {
            get 
            { 
                return this._device;
            }
            set 
            { 
                this._device = value;
                this._batch = new SpriteBatch(value);
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// For testing purposes only. Use GetInstance() instead.
        /// </summary>
        public SpriteBatchWrapper()
        {
            this._sm = ScreenManager.GetInstance();

        }

        public static SpriteBatchWrapper GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SpriteBatchWrapper();
            }
            if (_instance._sm == null) return null;
            return _instance;
        }

        public virtual void Begin()
        {
            if (_batch != null)
            {
                _batch.Begin();
            }
        }

        public virtual void End()
        {
            if (_batch != null)
            {
                _batch.End();
            }
        }

        public void Dispose()
        {
            _batch.Dispose();
        }

        //TODO: Make vectors represent top left and bottom right.
        public virtual void Draw(Texture2D texture, RectangleF rect, Color color)
        {
            _batch.Draw(texture, TranslateRectangle(rect), color);
        }

        public virtual void DrawString(SpriteFont font, String message, Vector2 pos, Color color)
        {
            float deltaxScale = this._device.Viewport.Width / this._nativewidth;
            float deltayScale = this._device.Viewport.Height / this._nativeheight;
            _batch.DrawString(font, message, TranslateVector(pos), color, 0.0f, Vector2.Zero, new Vector2(deltaxScale, deltayScale), SpriteEffects.None, 0f);
        }

        public virtual void DrawStringCentered(SpriteFont font, String message, Vector2 pos, Color color)
        {
            Vector2 center = TranslateVector(pos);
            float deltaxScale = this._device.Viewport.Width / this._nativewidth;
            float deltayScale = this._device.Viewport.Height / this._nativeheight;
            Vector2 stringSize = font.MeasureString(message);
            center -= new Vector2(stringSize.X * deltaxScale, stringSize.Y * deltayScale) / 2f;
            _batch.DrawString(font, message, center, color, 0.0f, Vector2.Zero, new Vector2(deltaxScale, deltayScale), SpriteEffects.None, 0f);                
        }

        public void DrawBackground(Texture2D texture, Color color)
        {
            this.Draw(texture, new RectangleF(0.0f, 0.0f, 1.0f, 1.0f), color);

        }

        private Rectangle TranslateRectangle(RectangleF origRect)
        {
            int width = _device.Viewport.Width;
            int height = _device.Viewport.Height;
            Rectangle rect = new Rectangle((int)(origRect.Left * width), (int)(origRect.Top * height), (int)(origRect.Width * width), (int)(origRect.Height * height));
            return rect;
        }

        private Vector2 TranslateVector(Vector2 vect)
        {
            int width = _device.Viewport.Width;
            int height = _device.Viewport.Height;
            return new Vector2(vect.X * width, vect.Y * height);
        }

        public virtual void DrawSprite(Texture2D texture, Rectangle srcRect, RectangleF destRect)
        {
            //Rectangle scaledSrcRect = srcRect.Scale(this._device.Viewport.Width, this._device.Viewport.Height);
            Rectangle scaledDestRect = this.Scale(destRect, this._device.Viewport.Width, this._device.Viewport.Height);
            this._batch.Draw(texture, scaledDestRect, srcRect, Color.White);
        }

        public virtual void DrawButton(Rectangle srcrect, RectangleF rect, Texture2D texture, String text, SpriteFont spriteFont, Color color)
        {
            DrawButton(srcrect, rect, texture, text, spriteFont, color, color);
        }

        public virtual void DrawButton(Rectangle srcrect, RectangleF rect, Texture2D texture, String text, SpriteFont spriteFont, Color textColor, Color maskColor)
        {
            if (_device == null) return;
            int width = _device.Viewport.Width;
            int height = _device.Viewport.Height;
            float x1 = rect.Left * width;
            float y1 = rect.Top * height;
            float x2 = rect.Right * width;
            float y2 = rect.Bottom * height;
            Rectangle destrect = new Rectangle((int)x1, (int)y1, (int)(x2 - x1), (int)(y2 - y1));
            Vector2 leftTop = new Vector2(x1, y1);
            Vector2 rightBottom = new Vector2(x2, y2);
            Vector2 centeredVect = ((leftTop + rightBottom) / 2) - (spriteFont.MeasureString(text) / 2);

            this._batch.Draw(texture, destrect, srcrect, maskColor);
            this.DrawStringCentered(spriteFont, text, new Vector2((rect.Left + rect.Right) / 2.0f, (rect.Top + rect.Bottom) / 2.0f), textColor);
        }

        private void Draw(Rectangle texbounds, Texture2D texture2D, RectangleF rect, Color color)
        {
            _batch.Draw(texture2D, TranslateRectangle(rect), texbounds, color);
        }

        public Texture2D GetBubbleSprite()
        {
            if (this._bubblesprite == null) this._bubblesprite = this._sm.Content.Load<Texture2D>(@"Sprites\bubbles");
            return this._bubblesprite;
        }

        internal SpriteFont GetBubbleSpriteFont()
        {
            if (this._bubblefont == null) this._bubblefont = this._sm.Content.Load<SpriteFont>(@"Fonts\BubbleFont");
            return this._bubblefont;
        } 

        private Rectangle Scale(RectangleF rect, int width, int height)
        {
            return new Rectangle((int)(rect.Left * width), (int)(rect.Top * height), (int)(rect.Width * width), (int)(rect.Height * height));
        }
        #endregion
    }
}
