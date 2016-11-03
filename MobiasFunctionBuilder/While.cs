using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MobiasFunctionBuilder.BodyLines;
using MobiasFunctionBuilder.Interface;
using MobiasFunctionBuilder.Parser;

namespace MobiasFunctionBuilder
{
    public class While : IWhile, IBodyLine
    {
        internal List<IBodyLine> BodyLines;
        internal Condition Condition;

        internal While(Condition condition)
        {
            if (condition == null) throw new ArgumentException();
            Condition = condition;
            BodyLines = new List<IBodyLine>();
        }

        public IBodyLine Do(IBodyLine firstCodeLine, params IBodyLine[] bodyLines)
        {
            BodyLines.Add(firstCodeLine);
            foreach (var bodyLine in bodyLines)
            {
                BodyLines.Add(bodyLine);
            }
            return this;
        }


        public string ToString(ParseContext context)
        {
            var result = "while(" + Condition.ToString(context) + ")\n";
            result += context.Pad + "{\n";
            context.AddLevel();

            foreach (var line in BodyLines)
            {
                var createVariable = line as CreateVariable;
                if (createVariable != null)
                {
                    createVariable.DefaultInitialize(context);
                }
                result += context.Pad + line.ToString(context) + ";\n";
            }

            context.RemoveLevel();
            result += context.Pad + "}";
            return result;
        }

        public void PreParseExpression(ParseContext context)
        {
            //var pl = context.Current;
            Condition.PreParseExpression(context);
            context.AddLevel();

            foreach (var line in BodyLines)
            {
                line.PreParseExpression(context);
            }

            context.RemoveLevel();
        }

        public Type ParsedType { get; private set; }

        public Expression ToExpression(ParseContext context)
        {
            var conditionExpression = Condition.ToExpression(context);
            context.AddLevel();

            var thenLine = new List<Expression>();
            var listOfThenVars = new List<ParameterExpression>();
            foreach (var line in BodyLines)
            {
                var expLine = line.ToExpression(context);

                var createVariable = line as CreateVariable;
                if (createVariable != null)
                {
                    listOfThenVars.Add((ParameterExpression)expLine);
                    expLine = createVariable.DefaultInitialize(context);
                }
                thenLine.Add(expLine);
            }
            var thenBlock = Expression.Block(listOfThenVars.ToArray(), thenLine);

            context.RemoveLevel();

            LabelTarget label = Expression.Label(Guid.NewGuid().ToString());
            var ifThenElse = Expression.IfThenElse(
                                                                conditionExpression,
                                                                thenBlock,
                                                                Expression.Break(label));
            return Expression.Loop(ifThenElse, label);
        }

      
    }
}
