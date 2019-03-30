using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace WhaleRiderSim
{
    /// <summary>
    /// BubbleFactory is responsible for the creation of bubbles.
    /// </summary>
    public class BubbleFactory
    {
        #region Properties
        //Represents movement per frame by % of screen
        private float _maxvelocity = 0.010f;
        private float _minvelocity = 0.002f;

        //Chance of spawning bubbles per frame
        private float _maxybase = 0.9f;
        private float _minybase = 0.3f;

        //Change in variables per frame
        private float _deltamaxvelocity = 0.00005f;
        private float _deltaminvelocity = 0.00005f;

        private Boolean _shouldmakebubble = false;
        public Boolean ShouldMakeBubble
        {
            get { return _shouldmakebubble; }
            set { _shouldmakebubble = value; }
        }

        private float[] _querytypefreq;

        private Random _rand;

        private float _minyvelocity = 0f;
        private float _maxyvelocity = .001f;
        private Parser _parser;

        private double _nextspawn = -1;
        private double _spawnrate = 4000;
        private double _spawnincrement = 100;
        private double _spawnvariance = 100;
        private double _maxspawnrate = 1000;
        #endregion

        #region Functions
        public BubbleFactory()
        {
            
            this._querytypefreq = new float[(int) QueryPartType.STAR + 1];
            this._rand = new Random();
            this._querytypefreq[(int)QueryPartType.DELETEINJECTION] = .1f;
            this._querytypefreq[(int)QueryPartType.DROPINJECTION] = .1f;
            this._querytypefreq[(int)QueryPartType.EQUALS] = .1f;
            this._querytypefreq[(int)QueryPartType.FROM] = .1f;
            this._querytypefreq[(int)QueryPartType.ID] = .15f;
            this._querytypefreq[(int)QueryPartType.SELECT] = .1f;
            this._querytypefreq[(int)QueryPartType.SEMICOLON] = .1f;
            this._querytypefreq[(int)QueryPartType.WHERE] = .1f;
            this._querytypefreq[(int)QueryPartType.STAR] = .15f;
        }

        public Bubble MakeBubble()
        {
            float Xspeed = (float)(this._minvelocity + this._rand.NextDouble() * (this._maxvelocity - this._minvelocity));
            float Yspeed = (float)(this._minyvelocity + this._rand.NextDouble() * (this._maxyvelocity - this._minyvelocity));
            Vector2 velocity = new Vector2(-Xspeed, Yspeed);
            Bubble b = new Bubble(velocity, this.RandomQueryPartType());
            b.SetPos(new Vector2(1.0f, (float)(this._minybase + this._rand.NextDouble() * (this._maxybase - this._minybase))));
            return b;
        }

        public QueryPartType RandomQueryPartType()
        {
            double r = this._rand.NextDouble();
            int i;
            for (i = 0; i < this._querytypefreq.Length && r > 0; i++)
            {

                r -= this._querytypefreq[i];
            }
            return (QueryPartType)(i - 1);

        }
        public void Update(GameTime gameTime)
        {
            if (this._nextspawn <= gameTime.TotalGameTime.TotalMilliseconds)
            {
                this._shouldmakebubble = true;
                this._nextspawn = gameTime.TotalGameTime.TotalMilliseconds + this._spawnrate + (this._rand.NextDouble() * 2 * this._spawnvariance - this._spawnvariance);
                if (this._spawnrate > this._maxspawnrate)
                {
                    this._spawnrate -= this._spawnincrement;
                }
                else
                {
                    this._maxvelocity += this._deltamaxvelocity;
                    this._minvelocity += this._deltaminvelocity;
                }
                if (this._parser != null)
                {
                    float remaining = 1f;
                    List<QueryPartType> validTypes = new List<QueryPartType>(this._parser.FindValidTypes());
                    float validWeight = (remaining - 0.3f) / (float)validTypes.Count;
                    float invalidWeight = (0.3f) / (float)(this._querytypefreq.Length - validTypes.Count);
                    for (int i = 0; i < this._querytypefreq.Length; i++)
                    {
                        if (validTypes.Contains((QueryPartType)i))
                        {
                            this._querytypefreq[i] = validWeight;
                        }
                        else
                        {
                            this._querytypefreq[i] = invalidWeight;
                        }
                    }
                }
            }
            else
            {
                this._shouldmakebubble = false;
            }
        }
        public void addParser(Parser parser)
        {
            this._parser = parser;
        } 
        #endregion
    }
}
