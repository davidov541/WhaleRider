using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhaleRiderSim
{
    public class Background
    {
        #region Properties
        /// <summary>
        /// Texture which will be displayed on the screen by this Background.
        /// </summary>
        private Texture _pic;
        public Texture Pic
        {
            get { return _pic; }
        } 
        #endregion

        #region Functions
        /// <summary>
        /// Default constructor which makes the held texture null. This makes an
        /// unusable Background object, so should be used with care.
        /// </summary>
        public Background()
        {
            this._pic = null;
        }

        /// <summary>
        /// Constructor which assigns the given texture to the Background object,
        /// setting it as the texture to draw when draw is called.
        /// </summary>
        /// <param name="t"></param>
        public Background(Texture t)
        {
            this._pic = t;
        }

        /// <summary>
        /// Draw method which is called for every frame. This method will draw
        /// the Pic field of the class to take up the entire screen.
        /// </summary>
        public void Draw(Color color)
        {
            SpriteBatchWrapper sbw = SpriteBatchWrapper.GetInstance();
            sbw.Begin();
            sbw.DrawBackground(this._pic as Texture2D, color);
            sbw.End();
        } 
        #endregion
    }
}
