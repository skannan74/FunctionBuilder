using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MobiasFunctionBuilder.Interface;
using MobiasFunctionBuilder.Operations;
using MobiasFunctionBuilder.Parser;
using MobiasFunctionBuilder.Utils;

namespace MobiasFunctionBuilder.BodyLines
{
    public class OperationInvoke : IOperation, IBodyLine
    {
        internal IOperation Variable;
        internal string MethodName;
        internal IOperation[] Parameters;
        internal readonly Type StaticDataType;

        public OperationInvoke(IOperation variable, string methodName, IOperation[] parameters)
        {
            Variable = variable;
            MethodName = methodName;
            Parameters = parameters;
        }

        public OperationInvoke(Type dataType, string methodName, IOperation[] parameters)
        {
            MethodName = methodName;
            Parameters = parameters;
            StaticDataType = dataType;
        }

        public string ToString(ParseContext context)
        {
            var result = string.Empty;
            if (StaticDataType == null)
            {
                result = Variable.ToString(context);
            }
            else
            {
                result = ReflectionUtil.TypeToString(StaticDataType);
            }
            result += "." + MethodName + "(";
            for (int i = 0; i < Parameters.Length; i++)
            {
                if (i > 0) result += ", ";
                result += Parameters[i].ToString(context);
            }
            return result + ")";
        }



        private List<Type> _paramTypes;

        public void PreParseExpression(ParseContext context)
        {
            _paramTypes = new List<Type>();
            for (int i = 0; i < Parameters.Length; i++)
            {
                Parameters[i].PreParseExpression(context);
                _paramTypes.Add(Parameters[i].ParsedType);
            }

            ParsedType = null;
        }
        public Type ParsedType { get; private set; }

        public Expression ToExpression(ParseContext context)
        {
            var pars = new List<Expression>();

            foreach (var param in Parameters)
            {
                pars.Add(param.ToExpression(context));
            }

            Type type = StaticDataType;
            if (StaticDataType == null)
            {
                type = Variable.ParsedType;
                if (Variable is OperationVariable)
                {
                    var variable = context.GetVariable(((OperationVariable)Variable).Name);
                    type = variable.DataType;
                }
            }

            var method = ReflectionUtil.GetMethod(type, MethodName, _paramTypes);

            if (method.GoodFrom >= 0)
            {
                var startDefault = method.GoodFrom;
                while (startDefault < method.ParamValues.Count)
                {
                    pars.Add(Operation.Constant(method.ParamValues[method.GoodFrom]).ToExpression(context));
                    startDefault++;
                }
            }
            var my = (MethodInfo)method.Method;
            if ((my.Attributes & MethodAttributes.Static) == 0)
            {
                return Expression.Call(Variable.ToExpression(context), method.Method as MethodInfo, pars);
            }
            return Expression.Call(method.Method as MethodInfo, pars);
        }
    }
}
