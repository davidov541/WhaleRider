using Microsoft.Xna.Framework.Input;

namespace WhaleRiderSim
{
    public class KeyboardProxy
    {
        #region Functions
        public KeyboardProxy()
        {

        }

        virtual public KeyboardState GetState()
        {
            return Keyboard.GetState();
        } 
        #endregion
    }
}
