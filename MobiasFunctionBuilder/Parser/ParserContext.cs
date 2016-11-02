using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MobiasFunctionBuilder.Parser
{
    public class ParseContext
    {
        internal Expression Return;

        private readonly List<ParseLevel> _parseLevels;

        internal string ReturnVariable;

        internal LabelTarget ReturnLabel;

        public ParseContext()
        {
            _parseLevels = new List<ParseLevel>();
        }

        internal int Count
        {
            get
            {
                return _parseLevels.Count;
            }
        }

        internal int Level
        {
            get
            {
                return Count - 1;
            }
        }

        internal ParseLevel Current
        {
            get
            {
                return _parseLevels[Level];
            }
        }

        internal Variable GetVariable(string name)
        {
            int i = Count - 1;
            while (i >= 0)
            {
                if (_parseLevels[i].HasVariable(name))
                {
                    return _parseLevels[i].GetVariable(name);
                }
                i--;
            }
            throw new Exception("Variable not found " + name);
        }

        internal bool HasVariable(Variable var)
        {
            int i = Count - 1;
            while (i >= 0)
            {
                if (_parseLevels[i].HasVariable(var.Name))
                {
                    return true;
                }
                i--;
            }
            return false;
        }

        internal ParseLevel AddLevel()
        {
            var pl = new ParseLevel(this);
            _parseLevels.Add(pl);
            return pl;
        }
        public void RemoveLevel()
        {
            _parseLevels.RemoveAt(Level);
        }

    }
}
