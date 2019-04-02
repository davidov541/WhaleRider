using System;
using System.Collections.Generic;
using System.Linq;

namespace WhaleRiderSim
{
    #region Enumerations
    public enum QueryPartType
    {
        SELECT = 0,
        FROM = 1,
        ID = 2,
        WHERE = 3,
        EQUALS = 4,
        DROPINJECTION = 5,
        DELETEINJECTION = 6,
        SEMICOLON = 7,
        STAR = 8
    } 
    #endregion

    public class QueryPart
    {
        #region Properties
        private String _content;
        public String Content
        {
            get;
            set;
        }

        private QueryPartType _type;
        public QueryPartType Type
        {
            get;
            set;
        }

        private static String[][] _validcontent =
        {
            new String[] {"SELECT"},
            new String[] {"FROM"},
            new String[] {"Sriram", "Whales", "Riding", "Id", "Databases", "Foo", "Bar", "Willy", "WP7", "Robert"},
            new String[] {"WHERE"},
            new String[] {"="},
            new String[] {"Sriram;DROP", "Whale;DROP", "Ride;DROP", "Id;DROP", "DB;DROP", "Foo;DROP", "Bar;DROP", "Willy;DROP", "WP7;DROP", "Rob;DROP"},
            new String[] {"Ride;DELETE", "Id;DELETE", "DB;DELETE", "Foo;DELETE", "Bar;DELETE", "WP7;DELETE", "Rob;DELETE"},
            new String[] {";"},
            new String[] {"*"}
        }; 
        #endregion

        #region Functions
        public QueryPart()
        { }

        public QueryPart(String content, QueryPartType type)
        {
            this.Content = content;
            this.Type = type;

        }
        public QueryPart(QueryPartType querytype, int random)
        {
            this.Type = querytype;
            this.Content = _validcontent[(int)querytype][random % _validcontent[(int)querytype].Length];

        }
        public override bool Equals(object query)
        {
            return (query.GetType() == this.GetType() && this.Type.Equals(((QueryPart)query).Type) && this.Content.Equals(((QueryPart)query).Content));
        }

        public override string ToString()
        {
            return "{QueryPart| (Type: " + this.Type.ToString() + "), (Content: " + this.Content.ToString() + ")}";
        } 
        #endregion
    }

    /*
     * NOTE: The grammar for the Parser is as follows:
     *      <Query> :== SELECT <id> FROM <id> [WHERE <id> = <id>]; 
     *              :== DELETE <id> FROM <id> [WHERE <id> = <id>];
     *              :== DROP <id>;

     *      Scoring is as follows:
     *          ---50 Points for each query part collected not part of an invalid query or injection attack.
     *          ---25 points lost for each query part which is part of an invalid query.
     *          ---Automatic 100 point deduction for any injection attack.
     *          ---25 points for each query part which is part of a valid query but is also part of an injection attack (either before or after).
     * */
    public class Parser
    {
        #region Properties
        private List<QueryPart> _parts;
        internal List<QueryPart> Parts
        {
            get { return _parts; }
            set { _parts = value; }
        }

        private readonly QueryPart _semicolon = new QueryPart(";", QueryPartType.SEMICOLON); 
        #endregion

        #region Functions
        public Parser()
        {
            this._parts = new List<QueryPart>();
        }

        public virtual int AddQuery(QueryPart part, out bool isValid)
        {
            this._parts.Add(part);
            isValid = true;
            float oldScore = 0f;
            float score = 0f;
            
            Stack<QueryPart> lastParts = new Stack<QueryPart>();
            foreach (QueryPart query in this._parts)
            {
                switch (query.Type)
                {
                    case QueryPartType.SELECT:
                        if (lastParts.Count > 0)
                        {
                            isValid = false;
                        }
                        break;
                    case QueryPartType.FROM:
                        if (lastParts.Count < 2)
                        {
                            isValid = false;
                            break;
                        }
                        QueryPart last = lastParts.Pop();
                        QueryPart secondLast = lastParts.Peek();
                        if ((last.Type != QueryPartType.ID && last.Type != QueryPartType.STAR) || (secondLast.Type != QueryPartType.SELECT && secondLast.Type != QueryPartType.DELETEINJECTION))
                        {
                            isValid = false;
                        }
                        lastParts.Push(last);
                        break;
                    case QueryPartType.ID:
                        if (lastParts.Count == 0 || lastParts.Count > 0 && lastParts.Peek().Type == QueryPartType.ID)
                        {
                            isValid = false;
                        }
                        break;
                    case QueryPartType.STAR:
                        if (lastParts.Count == 0 || (lastParts.Peek().Type != QueryPartType.SELECT && lastParts.Peek().Type != QueryPartType.DELETEINJECTION && lastParts.Peek().Type != QueryPartType.DROPINJECTION))
                        {
                            isValid = false;
                        }
                        break;
                    case QueryPartType.WHERE:
                        if (lastParts.Count < 2)
                        {
                            isValid = false;
                            break;
                        }
                        last = lastParts.Pop();
                        secondLast = lastParts.Peek();
                        if (last.Type != QueryPartType.ID || secondLast.Type != QueryPartType.FROM)
                        {
                            isValid = false;
                        }
                        lastParts.Push(last);
                        break;
                    case QueryPartType.EQUALS:
                        if (lastParts.Count < 2)
                        {
                            isValid = false;
                            break;
                        }
                        last = lastParts.Pop();
                        secondLast = lastParts.Peek();
                        if (last.Type != QueryPartType.ID || secondLast.Type != QueryPartType.WHERE)
                        {
                            isValid = false;
                        }
                        lastParts.Push(last);
                        break;
                    case QueryPartType.DELETEINJECTION:
                    case QueryPartType.DROPINJECTION:
                        if (isValid)
                        {
                            oldScore = (int) ((float) score * 0.5);
                        }
                        else
                        {
                            oldScore = (int)((float)score * -0.5);
                        }
                        score = 0;
                        if(lastParts.Count == 0)
                        {
                            isValid = false;
                            break;
                        }
                        last = lastParts.Peek();
                        if (lastParts.Count < 1 || last.Type != QueryPartType.EQUALS && last.Type != QueryPartType.FROM)
                        {
                            isValid = false;
                        }
                        break;
                    case QueryPartType.SEMICOLON:
                        if (lastParts.Count < 2)
                        {
                            isValid = false;
                            break;
                        }
                        last = lastParts.Pop();
                        secondLast = lastParts.Peek();
                        if (last.Type != QueryPartType.ID)
                        {
                            isValid = false;
                        }
                        else if (secondLast.Type != QueryPartType.FROM && secondLast.Type != QueryPartType.EQUALS && secondLast.Type != QueryPartType.DROPINJECTION)
                        {
                            isValid = false;
                        }
                        lastParts.Push(last);
                        break;
                    default:
                        break;
                }
                lastParts.Push(query);
                score += 50;
            }
            if (part.Type == QueryPartType.SEMICOLON || !isValid)
            {
                if (part.Type == QueryPartType.SEMICOLON && oldScore > 0)
                {
                    score = score * 0.5f + oldScore;
                }
                else if(!isValid)
                {
                    score = score * -0.5f + oldScore;
                }
                if (this._parts.Any((x => x.Type == QueryPartType.DELETEINJECTION || x.Type == QueryPartType.DROPINJECTION)))
                {
                    score -= 100;
                }
                if (score > 0)
                {
                    SoundManager.GetInstance(null).PlaySound("SuccessCompressed");
                }
                else
                {
                    SoundManager.GetInstance(null).PlaySound("BuzzerCompressed");
                }
                this._parts.Clear();
                return (int) score;
            }
            return 0;
        }

        public QueryPartType[] FindValidTypes()
        {
            QueryPartType[] validtypes;
            if (this._parts.Count == 0)
                return new QueryPartType[] { QueryPartType.SELECT };
            switch (this._parts.Last<QueryPart>().Type)
            {
                case QueryPartType.DELETEINJECTION:
                    validtypes = new QueryPartType[] { QueryPartType.ID, QueryPartType.STAR };
                    break;
                case QueryPartType.DROPINJECTION:
                    validtypes = new QueryPartType[] { QueryPartType.ID, QueryPartType.STAR };
                    break;
                case QueryPartType.EQUALS:
                    validtypes = new QueryPartType[] { QueryPartType.ID, QueryPartType.DROPINJECTION, QueryPartType.DELETEINJECTION };
                    break;
                case QueryPartType.FROM:
                    validtypes = new QueryPartType[] { QueryPartType.ID, QueryPartType.DROPINJECTION, QueryPartType.DELETEINJECTION };
                    break;
                case QueryPartType.ID:
                case QueryPartType.STAR:
                    QueryPartType secondLastPartType = this._parts.ElementAt(this._parts.Count - 2).Type;
                    if (secondLastPartType == QueryPartType.EQUALS)
                    {
                        validtypes = new QueryPartType[] { QueryPartType.SEMICOLON};
                    }
                    else if (secondLastPartType == QueryPartType.SELECT || secondLastPartType == QueryPartType.DELETEINJECTION)
                    {
                        validtypes = new QueryPartType[] { QueryPartType.FROM };
                    }
                    else if (secondLastPartType == QueryPartType.DROPINJECTION)
                    {
                        validtypes = new QueryPartType[] { QueryPartType.SEMICOLON };
                    }
                    else if (secondLastPartType == QueryPartType.FROM)
                    {
                        validtypes = new QueryPartType[] { QueryPartType.SEMICOLON, QueryPartType.WHERE };
                    }
                    else
                    {
                        validtypes = new QueryPartType[] { QueryPartType.EQUALS };
                    }
                    break;
                case QueryPartType.SELECT:
                    validtypes = new QueryPartType[] { QueryPartType.ID, QueryPartType.STAR };
                    break;
                case QueryPartType.WHERE:
                    validtypes = new QueryPartType[] { QueryPartType.ID };
                    break;
                default:
                    validtypes = new QueryPartType[] { QueryPartType.SELECT };
                    break;

            }
            return validtypes;
        }

        public String GetQueryString()
        {
            String returnable = "";
            foreach (QueryPart query in this._parts)
            {
                returnable += query.Content + " ";
            }
            if (returnable.Length > 0)
            {
                returnable = returnable.Remove(returnable.Length - 1);
            }
            return returnable;
        } 
        #endregion

    }
}
