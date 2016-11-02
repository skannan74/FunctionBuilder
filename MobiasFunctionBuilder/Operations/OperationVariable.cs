using MobiasFunctionBuilder.Interface;
using MobiasFunctionBuilder.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MobiasFunctionBuilder.Operations
{
    public class OperationVariable : ILeftRightable
    {
        internal string Name;

        public OperationVariable(string name)
        {
            Name = name;
        }

        public string ToString(ParseContext context)
        {
            return Name;
        }

        public Type ParsedType { get; private set; }

        public Expression ToExpression(ParseContext context)
        {
            var variable = context.GetVariable(Name);
            if (variable != null)
            {
                if (variable.Expression == null)
                {
                    variable.Expression = Expression.Parameter(ParsedType, Name);
                }
                return variable.Expression;
            }
            return Expression.Parameter(ParsedType, Name);
        }

        public void PreParseExpression(ParseContext context)
        {
            var resultVar = context.GetVariable(Name);
            ParsedType = resultVar.DataType;
        }
    }
}
