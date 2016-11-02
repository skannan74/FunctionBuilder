using System;
using System.Linq.Expressions;

namespace MobiasFunctionBuilder.Parser
{
    public interface IParsable
    {
        string ToString(ParseContext context);
        Expression ToExpression(ParseContext context);

        void PreParseExpression(ParseContext context);
        Type ParsedType { get; }
    }
}