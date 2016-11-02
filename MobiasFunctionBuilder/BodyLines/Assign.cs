using MobiasFunctionBuilder.Enums;
using MobiasFunctionBuilder.Interface;
using MobiasFunctionBuilder.Parser;
using System.ComponentModel;
using System;
using System.Linq.Expressions;

namespace MobiasFunctionBuilder.BodyLines
{
    public class Assign : IBodyLine
    {
        internal ILeftable LValue;
        internal IRightable RValue;
        internal AssignementOperator AssignType;

        public Type ParsedType { get; private set; }

        public Assign(ILeftable lvalue, IRightable rValue, AssignementOperator assignType)
        {
            LValue = lvalue;
            RValue = rValue;
            AssignType = assignType;
        }

        public string ToString(ParseContext context)
        {
            var rstring = RValue.ToString(context);
            var lstring = LValue.ToString(context);
            return lstring + " " + AssignementToString() + " " + rstring;
        }

        private string AssignementToString()
        {
            switch (AssignType)
            {
                case (AssignementOperator.Assign):
                    return "=";
                case (AssignementOperator.MultiplyAssign):
                    return "*=";
                case (AssignementOperator.SubtractAssign):
                    return "-=";
                case (AssignementOperator.SumAssign):
                    return "+=";
            }
            throw new InvalidEnumArgumentException();
        }

        public Expression ToExpression(ParseContext context)
        {
            switch (AssignType)
            {
                case (AssignementOperator.Assign):
                    {
                        return Expression.Assign(LValue.ToExpression(context), Expression.Convert(RValue.ToExpression(context), LValue.ParsedType));
                    }
                case (AssignementOperator.MultiplyAssign):
                    return Expression.MultiplyAssign(LValue.ToExpression(context), RValue.ToExpression(context));
                case (AssignementOperator.SubtractAssign):
                    return Expression.SubtractAssign(LValue.ToExpression(context), RValue.ToExpression(context));
                case (AssignementOperator.SumAssign):
                    {
                        if (LValue.ParsedType == typeof(string) || RValue.ParsedType == typeof(string))
                        {
                            Expression<Func<object, object, string>> func = ((a, b) => SumAssign(a, b));
                            return Expression.Assign(LValue.ToExpression(context),
                                                                Expression.Invoke(func, LValue.ToExpression(context), RValue.ToExpression(context)));

                        }
                        return Expression.AddAssign(LValue.ToExpression(context), RValue.ToExpression(context));
                    }
            }
            throw new InvalidEnumArgumentException();
        }

        private static string SumAssign(object a, object b)
        {
            return a.ToString() + b.ToString();
        }

        public void PreParseExpression(ParseContext context)
        {
            RValue.PreParseExpression(context);
            LValue.PreParseExpression(context);

            ParsedType = LValue.ParsedType;
        }
    }


}
