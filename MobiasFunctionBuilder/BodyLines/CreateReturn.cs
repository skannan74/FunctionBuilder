using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MobiasFunctionBuilder.Interface;
using MobiasFunctionBuilder.Parser;

namespace MobiasFunctionBuilder.BodyLines
{
    public class CreateReturn : IBodyLine
    {
        internal CreateReturn()
        {
            ParsedType = null;
        }

        public string ToString(ParseContext context)
        {
            if (!string.IsNullOrWhiteSpace(context.ReturnVariable))
            {
                return "return " + context.ReturnVariable;
            }
            return "return";
        }

        public Expression ToExpression(ParseContext context)
        {
            return Expression.Goto(context.ReturnLabel);
        }

        public void PreParseExpression(ParseContext context)
        {

        }

        public Type ParsedType { get; private set; }
    }
}
