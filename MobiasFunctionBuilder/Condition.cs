using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MobiasFunctionBuilder.Interface;
using MobiasFunctionBuilder.Parser;

namespace MobiasFunctionBuilder
{
    public abstract class Condition : IParsable, IRightable
    {
        protected Condition()
        {
        }

        public abstract string ToString(ParseContext context);
        public abstract Expression ToExpression(ParseContext context);
        public abstract void PreParseExpression(ParseContext context);
        public abstract Type ParsedType { get; protected set; }
    }
}
