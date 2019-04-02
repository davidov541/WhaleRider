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
        private float _minybase = 0.35f;

        //Change in variables per frame
        private float _deltamaxvelocity = 0.00005f;
        private float _deltaminvelocity = 0.00005f;

        private float[] _querytypefreq;

        private Random _rand;

        private float _minyvelocity = 0f;
        private float _maxyvelocity = .001f;

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
            this._querytypefreq[(int)QueryPartType.DROPINJECTION] = 0.9f;// .1f;
            this._querytypefreq[(int)QueryPartType.EQUALS] = 0.0f;//.1f;
            this._querytypefreq[(int)QueryPartType.FROM] = 0.0f;// .1f;
            this._querytypefreq[(int)QueryPartType.ID] = 0.0f;// .15f;
            this._querytypefreq[(int)QueryPartType.SELECT] = 0.0f;// .1f;
            this._querytypefreq[(int)QueryPartType.SEMICOLON] = 0.0f;// .1f;
            this._querytypefreq[(int)QueryPartType.WHERE] = 0.0f;// .1f;
            this._querytypefreq[(int)QueryPartType.STAR] = 0.0f;// .15f;
        }

        public Bubble MakeBubble(GameTime gameTime, Parser parser)
        {
            if (this._nextspawn <= gameTime.TotalGameTime.TotalMilliseconds)
            {
                int i;

                //Get random query type to make next bubble have.
                List<QueryPartType> validTypes = new List<QueryPartType>(parser.FindValidTypes());
                float validWeight = (1.0f - 0.3f) / (float)validTypes.Count;
                float invalidWeight = (0.3f) / (float)(this._querytypefreq.Length - validTypes.Count);
                double r = this._rand.NextDouble();
                for (i = 0; i < this._querytypefreq.Length && r > 0; i++)
                {
                    if (validTypes.Contains((QueryPartType)i))
                    {
                        r -= validWeight;
                    }
                    else
                    {
                        r -= invalidWeight;
                    }
                }
                QueryPartType type = (QueryPartType)(i - 1);

                //Get velocity of new bubble.
                float Xspeed = (float)(this._minvelocity + this._rand.NextDouble() * (this._maxvelocity - this._minvelocity));
                float Yspeed = (float)(this._minyvelocity + this._rand.NextDouble() * (this._maxyvelocity - this._minyvelocity));
                Vector2 velocity = new Vector2(-Xspeed, Yspeed);
                
                //Make bubble instance and set position.
                Bubble b = new Bubble(velocity, type, validTypes.Contains(type) && ConfigurationManager.GetInstance().Train);
                b.SetPos(new Vector2(1.0f, (float)(this._minybase + this._rand.NextDouble() * (this._maxybase - this._minybase))));

                //Adjust settings for next bubble
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
                return b;
            }
            return null;
        }
        #endregion
    }
}
