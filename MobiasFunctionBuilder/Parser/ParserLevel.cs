using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiasFunctionBuilder.Parser
{
    internal class ParseLevel
    {
        private readonly ParseContext _parseContext;
        private readonly Dictionary<string, Variable> _variables;

        public ParseLevel(ParseContext parseContext)
        {
            _variables = new Dictionary<string, Variable>(StringComparer.InvariantCultureIgnoreCase);
            _parseContext = parseContext;
        }

        internal void AddVariable(Variable var)
        {
            if (_parseContext.HasVariable(var)) throw new Exception("Duplicate variable");
            _variables.Add(var.Name, var);
        }

        internal bool HasVariable(string name)
        {
            // if name has dot, assume name is available and return true.
            //it is expensive to loop thru and see whether the variable is there or not.
            //anyway we are going to loop to get the property type later.
            //if name doesn't have dot, property should be in input parameter.
            if (name.Contains(("."))) 
                return true;

            return _variables.ContainsKey(name);
        }


        internal Variable GetVariable(string name)
        {
            return _variables[name];
        }


    }
}
