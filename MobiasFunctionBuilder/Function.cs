using MobiasFunctionBuilder.Interface;
using MobiasFunctionBuilder.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Threading;
using MobiasFunctionBuilder.BodyLines;

namespace MobiasFunctionBuilder
{
    public class Function : IBodyOrParameter, IFunctionReturn, IParsable
    {
        #region Member Variables

        internal bool _asConstructor;

        internal string _functionName;

        internal string _returnVariable;

        internal readonly Dictionary<string, Variable> _inputParameters;

        private readonly List<IBodyLine> _bodyLines;

        public Type ParsedType
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        private Type _returnDataType;

        #endregion

        protected Function(string functionName, bool asConstructor = false)
        {
            _inputParameters = new Dictionary<string, Variable>();
            _bodyLines = new List<IBodyLine>();
            _functionName = functionName;
            _asConstructor = asConstructor;
        }

        public static IBodyOrParameter Create(string name = "Call", bool asConstructor = false)
        {
            return new Function(name, asConstructor);
        }

        public IBodyOrParameter InputParameter(Type type, string name)
        {
            if (_inputParameters.ContainsKey(name))
            {
                throw new ArgumentException($"Duplicate variable {nameof(name)}");
            }
            _inputParameters.Add(name, new Variable(type, name, false));
            return this;
        }

        public IBodyOrParameter InputParameter<TData>(string name)
        {
            return InputParameter(typeof(TData), name);
        }

        public IFunctionReturn Body(params IBodyLine[] bodyLines)
        {
            _bodyLines.AddRange(bodyLines);

            return this;
        }

        public IExpressionResult Returns(string variableName)
        {
            _returnVariable = variableName;
            return this;
        }

        public LambdaExpression ToExpression()
        {
            var ctx = new ParseContext();
            ctx.AddLevel();
            return (LambdaExpression)ToExpression(ctx);
        }

        public TData ToLambda<TData>() where TData : class
        {
            var resultExpression = ToExpression();
            var compiled = resultExpression.Compile();
            return compiled as TData;
        }

        public string ToString(ParseContext context)
        {
            throw new NotImplementedException();
        }

        public Expression ToExpression(ParseContext context)
        {
           // PreParseExpression();
            var expressionsList = new List<Expression>();
            var exprParams = new List<ParameterExpression>();

            context.ReturnLabel = Expression.Label("return");
            context.Return = Expression.Goto(context.ReturnLabel);

            var returnLabelExpression = Expression.Label(context.ReturnLabel);

            var pl = context.Current;

            foreach (var param in _inputParameters)
            {
                var pv = param.Value;
                var exp = Expression.Parameter(pv.DataType, param.Key);
                pv.Expression = exp;
                pl.AddVariable(pv);
                exprParams.Add(exp);
            }


            var listOfVars = new List<ParameterExpression>();
            foreach (var bodyLine in _bodyLines)
            {
                var expr = bodyLine.ToExpression(context);

                var createVariable = bodyLine as CreateVariable;
                if (createVariable != null)
                {
                    listOfVars.Add((ParameterExpression)expr);
                    expr = createVariable.DefaultInitialize(context);
                }
                expressionsList.Add(expr);
            }

            expressionsList.Add(returnLabelExpression);

            if (!string.IsNullOrWhiteSpace(_returnVariable))
            {
                var resultVar = context.GetVariable(_returnVariable);
                expressionsList.Add(resultVar.Expression);
            }
            var block = Expression.Block(
                    listOfVars.ToArray(),
                    expressionsList);

            return Expression.Lambda(block, exprParams);
        }

        private void PreParseExpression()
        {
            var ctx = new ParseContext();
            ctx.AddLevel();
            PreParseExpression(ctx);
            ctx.RemoveLevel();
        }

        public void PreParseExpression(ParseContext context)
        {
            var pl = context.Current;
            foreach (var param in _inputParameters)
            {
                var pv = param.Value;
                pl.AddVariable(pv);
            }

            foreach (var bodyLine in _bodyLines)
            {
                bodyLine.PreParseExpression(context);
            }
            if (!string.IsNullOrWhiteSpace(_returnVariable))
            {
                var resultVar = context.GetVariable(_returnVariable);
                _returnDataType = resultVar.DataType;
            }
        }
    }
}
