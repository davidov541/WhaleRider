using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace WhaleRiderSim
{
    public class PhoneInputHandler : InputHandler
    {
        #region Properties
        private TouchCollection _pasttouchstate;
        public TouchCollection PastTouchState
        {
            get { return _pasttouchstate; }
            set { _pasttouchstate = value; }
        }

        private GestureSample? _lastgesture;
        public GestureSample? LastGesture
        {
            get { return _lastgesture; }
            set { _lastgesture = value; }
        }

        private TouchCollection _lastcollection;

        public TouchCollection LastTouchCollection
        {
            get { return _lastcollection; }
            set { _lastcollection = value; }
        }
        #endregion

        #region Functions
        /// <summary>
        /// For testing purposed only. Use GetInstance() Instead
        /// </summary>
        public PhoneInputHandler()
        {

            this._mouse = new MouseProxy();
            this._keyboard = new KeyboardProxy();
            this._lastcharpressed = '\0';
        }

        /// <summary>
        /// Checks to see if a click was registered during the last update. Will return the point
        /// if and only if the the left mouse button was pressed during the previous update but
        /// is was pressed during the most recent update. Returns null otherwise.
        /// </summary>
        /// <returns>Point, the point where the mouse was released</returns>
        public override Vector2? GetLeftClick()
        {
            if (null != this._device)
            {
                this._width = this._device.Viewport.Width;
                this._height = this._device.Viewport.Height;
            }
            if (this._lastgesture == null)
            {
                return null;
            }
            if (this.LastGesture.Value.GestureType == GestureType.Tap)
            {
                return new Vector2(this.LastGesture.Value.Position.X / this._width, this.LastGesture.Value.Position.Y / this._height);
            }
            return null;
        }

        /// <summary>
        /// Checks to see if the left mouse button was pressed during the most recent update.
        /// </summary>
        /// <returns>True if left button was pressed, false otherwise</returns>
        public override Boolean IsLeftMouseButtonDown()
        {
            return this.LastTouchCollection.Count != 0;
        }

        public override Boolean IsRightMouseButtonDown()
        {
            return this.LastGesture != null && this.LastGesture.Value.GestureType == GestureType.Hold;
        }

        public override Vector2 GetMousePosition()
        {
            if (null != this._device)
            {
                this._width = this._device.Viewport.Width;
                this._height = this._device.Viewport.Height;
            }
            TouchCollection currCollection = this._lastcollection;
            if (currCollection.Count == 0)
            {
                return Vector2.Zero;
            }
            return new Vector2(currCollection.First().Position.X / this._width, currCollection.First().Position.Y / this._height);            
        }

        public override Boolean IsKeyPressed(Keys key)
        {
            return this._currentkeyboardstate.IsKeyDown(key);
        }

        public override char GetKeyPressed()
        {
            if (this._lastcharpressed != '\0' && this._currentkeyboardstate.GetPressedKeys().Length == 0)
            {
                char c = this._lastcharpressed;
                this._lastcharpressed = '\0';
                return c;
            }
            else if (this._lastcharpressed == '\0' && this._currentkeyboardstate.GetPressedKeys().Length > 0)
            {
                Keys[] keys = this._currentkeyboardstate.GetPressedKeys();
                bool shift = this._currentkeyboardstate.IsKeyDown(Keys.LeftShift) || this._currentkeyboardstate.IsKeyDown(Keys.RightShift);
                switch (keys[0])
                {
                    //Alphabet keys
                    case Keys.A: if (shift) { this._lastcharpressed = 'A'; } else { this._lastcharpressed = 'a'; } break;
                    case Keys.B: if (shift) { this._lastcharpressed = 'B'; } else { this._lastcharpressed = 'b'; } break;
                    case Keys.C: if (shift) { this._lastcharpressed = 'C'; } else { this._lastcharpressed = 'c'; } break;
                    case Keys.D: if (shift) { this._lastcharpressed = 'D'; } else { this._lastcharpressed = 'd'; } break;
                    case Keys.E: if (shift) { this._lastcharpressed = 'E'; } else { this._lastcharpressed = 'e'; } break;
                    case Keys.F: if (shift) { this._lastcharpressed = 'F'; } else { this._lastcharpressed = 'f'; } break;
                    case Keys.G: if (shift) { this._lastcharpressed = 'G'; } else { this._lastcharpressed = 'g'; } break;
                    case Keys.H: if (shift) { this._lastcharpressed = 'H'; } else { this._lastcharpressed = 'h'; } break;
                    case Keys.I: if (shift) { this._lastcharpressed = 'I'; } else { this._lastcharpressed = 'i'; } break;
                    case Keys.J: if (shift) { this._lastcharpressed = 'J'; } else { this._lastcharpressed = 'j'; } break;
                    case Keys.K: if (shift) { this._lastcharpressed = 'K'; } else { this._lastcharpressed = 'k'; } break;
                    case Keys.L: if (shift) { this._lastcharpressed = 'L'; } else { this._lastcharpressed = 'l'; } break;
                    case Keys.M: if (shift) { this._lastcharpressed = 'M'; } else { this._lastcharpressed = 'm'; } break;
                    case Keys.N: if (shift) { this._lastcharpressed = 'N'; } else { this._lastcharpressed = 'n'; } break;
                    case Keys.O: if (shift) { this._lastcharpressed = 'O'; } else { this._lastcharpressed = 'o'; } break;
                    case Keys.P: if (shift) { this._lastcharpressed = 'P'; } else { this._lastcharpressed = 'p'; } break;
                    case Keys.Q: if (shift) { this._lastcharpressed = 'Q'; } else { this._lastcharpressed = 'q'; } break;
                    case Keys.R: if (shift) { this._lastcharpressed = 'R'; } else { this._lastcharpressed = 'r'; } break;
                    case Keys.S: if (shift) { this._lastcharpressed = 'S'; } else { this._lastcharpressed = 's'; } break;
                    case Keys.T: if (shift) { this._lastcharpressed = 'T'; } else { this._lastcharpressed = 't'; } break;
                    case Keys.U: if (shift) { this._lastcharpressed = 'U'; } else { this._lastcharpressed = 'u'; } break;
                    case Keys.V: if (shift) { this._lastcharpressed = 'V'; } else { this._lastcharpressed = 'v'; } break;
                    case Keys.W: if (shift) { this._lastcharpressed = 'W'; } else { this._lastcharpressed = 'w'; } break;
                    case Keys.X: if (shift) { this._lastcharpressed = 'X'; } else { this._lastcharpressed = 'x'; } break;
                    case Keys.Y: if (shift) { this._lastcharpressed = 'Y'; } else { this._lastcharpressed = 'y'; } break;
                    case Keys.Z: if (shift) { this._lastcharpressed = 'Z'; } else { this._lastcharpressed = 'z'; } break;

                    //Decimal keys
                    case Keys.D0: if (shift) { this._lastcharpressed = ')'; } else { this._lastcharpressed = '0'; } break;
                    case Keys.D1: if (shift) { this._lastcharpressed = '!'; } else { this._lastcharpressed = '1'; } break;
                    case Keys.D2: if (shift) { this._lastcharpressed = '@'; } else { this._lastcharpressed = '2'; } break;
                    case Keys.D3: if (shift) { this._lastcharpressed = '#'; } else { this._lastcharpressed = '3'; } break;
                    case Keys.D4: if (shift) { this._lastcharpressed = '$'; } else { this._lastcharpressed = '4'; } break;
                    case Keys.D5: if (shift) { this._lastcharpressed = '%'; } else { this._lastcharpressed = '5'; } break;
                    case Keys.D6: if (shift) { this._lastcharpressed = '^'; } else { this._lastcharpressed = '6'; } break;
                    case Keys.D7: if (shift) { this._lastcharpressed = '&'; } else { this._lastcharpressed = '7'; } break;
                    case Keys.D8: if (shift) { this._lastcharpressed = '*'; } else { this._lastcharpressed = '8'; } break;
                    case Keys.D9: if (shift) { this._lastcharpressed = '('; } else { this._lastcharpressed = '9'; } break;

                    //Decimal numpad keys
                    case Keys.NumPad0: this._lastcharpressed = '0'; break;
                    case Keys.NumPad1: this._lastcharpressed = '1'; break;
                    case Keys.NumPad2: this._lastcharpressed = '2'; break;
                    case Keys.NumPad3: this._lastcharpressed = '3'; break;
                    case Keys.NumPad4: this._lastcharpressed = '4'; break;
                    case Keys.NumPad5: this._lastcharpressed = '5'; break;
                    case Keys.NumPad6: this._lastcharpressed = '6'; break;
                    case Keys.NumPad7: this._lastcharpressed = '7'; break;
                    case Keys.NumPad8: this._lastcharpressed = '8'; break;
                    case Keys.NumPad9: this._lastcharpressed = '9'; break;

                    //Special keys
                    case Keys.OemTilde: if (shift) { this._lastcharpressed = '~'; } else { this._lastcharpressed = '`'; } break;
                    case Keys.OemSemicolon: if (shift) { this._lastcharpressed = ':'; } else { this._lastcharpressed = ';'; } break;
                    case Keys.OemQuotes: if (shift) { this._lastcharpressed = '"'; } else { this._lastcharpressed = '\''; } break;
                    case Keys.OemQuestion: if (shift) { this._lastcharpressed = '?'; } else { this._lastcharpressed = '/'; } break;
                    case Keys.OemPlus: if (shift) { this._lastcharpressed = '+'; } else { this._lastcharpressed = '='; } break;
                    case Keys.OemPipe: if (shift) { this._lastcharpressed = '|'; } else { this._lastcharpressed = '\\'; } break;
                    case Keys.OemPeriod: if (shift) { this._lastcharpressed = '>'; } else { this._lastcharpressed = '.'; } break;
                    case Keys.OemOpenBrackets: if (shift) { this._lastcharpressed = '{'; } else { this._lastcharpressed = '['; } break;
                    case Keys.OemCloseBrackets: if (shift) { this._lastcharpressed = '}'; } else { this._lastcharpressed = ']'; } break;
                    case Keys.OemMinus: if (shift) { this._lastcharpressed = '_'; } else { this._lastcharpressed = '-'; } break;
                    case Keys.OemComma: if (shift) { this._lastcharpressed = '<'; } else { this._lastcharpressed = ','; } break;
                    case Keys.Space: this._lastcharpressed = ' '; break;
                    case Keys.Back: this._lastcharpressed = (char)8; break;
                    default: this._lastcharpressed = '\0'; break;
                }
            }
            return '\0';
        }

        public override Boolean IsBackKeyPressed()
        {
            return GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed;
        }

        public override void Update()
        {
            this._previousmousestate = this._currentmousestate;
            this._currentmousestate = this._mouse.GetState();

            this._previouskeyboardstate = this._currentkeyboardstate;
            this._currentkeyboardstate = this._keyboard.GetState();

            TouchCollection currCollection = TouchPanel.GetState();
            if (!TouchPanel.IsGestureAvailable)
            {
                this._lastgesture = null;
            }
            else
            {
                this._lastgesture = TouchPanel.ReadGesture();
            }
            this._lastcollection = TouchPanel.GetState();
        }
        #endregion
    }
}
