using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WhaleRiderSim
{
    public class Bubble
    {
        #region Enumerations
        public enum Type
        {
            SmallGood = 0,
            LargeGood = 1,
            LargeBad  = 2,
            SmallHint = 3,
            LargeHint = 4
        }
        public enum State
        {
            Normal,
            Popping,
            Popped
        } 
        #endregion

        #region Properties
        //Radius of bubbles relative to screen size
        public static float SmallSize
        {
            get { return 0.04f; }
        }

        public static float LargeSize
        {
            get { return 0.063f; }
        }

        public float SpriteSize
        {
            get { return .096f; }
        }

        private Vector2 acceleration;

        //Sidelength of each Spite Block in spritemap
        private readonly static int _spriteboxsize = 125;
        private readonly static int _spriterawxsize = 102;
        private readonly static int _spriterawysize = 85;
        private readonly static int _smallspriterawxsize = 60;
        private readonly static int _smallspriterawysize = 51;

        private State _state;
        public State StateValue
        {
            get { return _state; }
            set { _state = value; }
        }

        private QueryPart _query;
        public QueryPart Query
        {
            get { return _query; }
            set { _query = value; }
        }

        private Bubble.Type _type;
        public Bubble.Type BubbleType
        {
            get { return _type; }
            set { _type = value; }
        }

        private Vector2 _velocity;
        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        private Random _rand;
        public Random Rand
        {
            get { return _rand; }
            set { _rand = value; }
        }

        private Vector2 _centerpos;
        public Vector2 CenterPos
        {
            get { return _centerpos; }
            set { _centerpos = value; }
        }

        private float _boundsize;
        public float BoundSize
        {
            get { return _boundsize; }
            set { _boundsize = value; }
        }

        private SpriteBatchWrapper _batch;
        public SpriteBatchWrapper Batch
        {
            get { return _batch; }
            set { _batch = value; }
        }

        private float _baseY;

        private Texture2D _sprite;
        private SpriteFont _font;
        private Rectangle _spriteboundssrc;
        private RectangleF _spriteboundsdest;
        private Color textColor; 
        #endregion

        #region Functions
        public Bubble(Vector2 velocity, QueryPartType querytype, Boolean isValid)
        {
            this._velocity = velocity;
            this._rand = new Random();
            this._centerpos = new Vector2(1f, 0.6f);
            this._state = Bubble.State.Normal;
            this._query = new QueryPart(querytype, this._rand.Next());
            this._type = this.ChooseType(this._query, isValid);
            this.acceleration = Vector2.Zero;
            SetValid(isValid);
            switch (this._type)
            {
                case Bubble.Type.SmallGood:
                case Bubble.Type.SmallHint:
                    this._boundsize = Bubble.SmallSize;
                    break;
                case Bubble.Type.LargeGood:
                case Bubble.Type.LargeBad:
                case Bubble.Type.LargeHint:
                    this._boundsize = Bubble.LargeSize;
                    break;
            }
            this._batch = SpriteBatchWrapper.GetInstance();

            if (_batch != null)
            {
                this._font = _batch.GetBubbleSpriteFont();
                this._sprite = _batch.GetBubbleSprite();
            }
            Vector2 offset = new Vector2(this._boundsize, this._boundsize);

        }

        public override bool Equals(object obj)
        {
            Bubble other = (Bubble)obj;
            if (!this._boundsize.Equals(other._boundsize))
            {
                return false;
            }
            if (!this._centerpos.Equals(other._centerpos))
            {
                return false;
            }
            if (this._font != null && !this._font.Equals(other._font))
            {
                return false;
            }
            if (this._font == null && other._font != null)
            {
                return false;
            }
            if (!this._query.Equals(other._query))
            {
                return false;
            }
            if (this._sprite != null && !this._sprite.Equals(other._sprite))
            {
                return false;
            }
            if (this._sprite == null && other._sprite != null)
            {
                return false;
            }
            if (!this._spriteboundssrc.Equals(other._spriteboundssrc))
            {
                return false;
            }
            if (!this._state.Equals(other._state))
            {
                return false;
            }
            if (this._batch != null && !this._batch.Equals(other._batch))
            {
                return false;
            }
            if (this._batch == null && other._batch != null)
            {
                return false;
            }
            if (!this._type.Equals(other._type))
            {
                return false;
            }
            if (!this._velocity.Equals(other._velocity))
            {
                return false;
            }
            return true;
        }

        public QueryPart Pop()
        {
            this._state = Bubble.State.Popping;
            return this._query;
        }

        /// <summary>
        /// Sets the center position of the bubble to pos.
        /// </summary>
        /// <param name="pos"></param>
        public void SetPos(Vector2 pos)
        {
            this._centerpos = new Vector2(pos.X, pos.Y);
            Vector2 offset = new Vector2(this._boundsize, this._boundsize * 1.2f);
            Vector2 topleft = this._centerpos - offset;
            this._spriteboundsdest = new RectangleF(this._centerpos.X - offset.X, this._centerpos.Y - offset.Y, offset.X * 2, offset.Y * 2);
            this._baseY = pos.Y;
        }

        public Bubble.Type ChooseType(QueryPart query, Boolean isValid)
        {
            if (query.Type == QueryPartType.DELETEINJECTION ||
                query.Type == QueryPartType.DROPINJECTION)
            {
                return Bubble.Type.LargeBad;
            }

            if (query.Content.Length <= 3)
            {
                if (isValid)
                    return Bubble.Type.SmallHint;
                return Bubble.Type.SmallGood;
            }
            if (isValid)
                return Bubble.Type.LargeHint;
            return Bubble.Type.LargeGood;
        }

        public void Draw()
        {
            SpriteBatchWrapper sbw = SpriteBatchWrapper.GetInstance();
            sbw.Begin();
            sbw.DrawSprite(this._sprite, this._spriteboundssrc, this._spriteboundsdest);
            sbw.DrawStringCentered(this._font, this._query.Content, this._centerpos, this.textColor);
            sbw.End();
        }

        public void Update()
        {
            this.acceleration.Y = .001f * (this._baseY - this._centerpos.Y);
            this.Velocity += acceleration;
            this._centerpos += this._velocity;
            if (this._type == Type.LargeHint || this._type == Type.SmallHint)
            {
                this._spriteboundssrc = new Rectangle(0,
                    (int)(this._type - 3) * Bubble._spriteboxsize,
                    (this._type == Type.SmallGood || this._type == Type.SmallHint) ? Bubble._smallspriterawxsize : Bubble._spriterawxsize,
                    (this._type == Type.SmallGood || this._type == Type.SmallHint) ? Bubble._smallspriterawysize : Bubble._spriterawysize);
            }
            else
            {
                this._spriteboundssrc = new Rectangle(0,
                    (int)this._type * Bubble._spriteboxsize,
                    (this._type == Type.SmallGood || this._type == Type.SmallHint) ? Bubble._smallspriterawxsize : Bubble._spriterawxsize,
                    (this._type == Type.SmallGood || this._type == Type.SmallHint) ? Bubble._smallspriterawysize : Bubble._spriterawysize);

            }
            Vector2 offset;

            if (SpriteBatchWrapper.GetInstance() != null)
            {
                offset = new Vector2(this._boundsize, this._boundsize * 1.2f);
            }
            else
            {
                offset = Vector2.Zero;
            }
            Vector2 topleft = this._centerpos - offset;
            this._spriteboundsdest = new RectangleF(this._centerpos.X - offset.X, this._centerpos.Y - offset.Y, offset.X * 2, offset.Y * 2);
        }

        //Assumes the bounds are larger than the bubble
        public virtual Boolean Intersects(RectangleF rect)
        {
            CircleF collisionCirc;
            if (this._type == Bubble.Type.SmallGood)
            {
                 collisionCirc = new CircleF(this._centerpos, .08f);
            }
            else
            {
                collisionCirc = new CircleF(this._centerpos, .12f);
            }
            return collisionCirc.IntersectsWith(rect);
        }

        public void SetValid(Boolean isValid)
        {
            if (this.Query.Type == QueryPartType.DELETEINJECTION || this.Query.Type == QueryPartType.DROPINJECTION)
            {
                this.textColor = Color.WhiteSmoke;
            }
            else if (isValid)
            {
                this.textColor = Color.Orange;
            }
            else
            {
                this.textColor = Color.Navy;
            }
            this._type = this.ChooseType(this._query, isValid);
        }

        #endregion
    }
}
