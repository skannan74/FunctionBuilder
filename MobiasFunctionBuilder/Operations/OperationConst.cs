using MobiasFunctionBuilder.Interface;
using MobiasFunctionBuilder.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace MobiasFunctionBuilder.Operations
{
    public class OperationConst : IRightable
    {
        private readonly object _value;

        public OperationConst(object value)
        {
            _value = value;
        }

        public Type ParsedType { get; private set; }

        public Expression ToExpression(ParseContext context)
        {
            return Expression.Constant(_value);
        }

        public string ToString(ParseContext context)
        {
            if (_value == null) return "null";
            var type = _value.GetType();
            if (type.IsValueType || type.IsEnum)
            {
                return _value.ToString();
            }
            return "\"" + _value + "\"";
        }

        public void PreParseExpression(ParseContext context)
        {
            if (_value == null)
                ParsedType = typeof(object);
            else
                ParsedType = _value.GetType();
        }
    }
}
