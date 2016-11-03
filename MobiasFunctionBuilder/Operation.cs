using MobiasFunctionBuilder.Interface;
using MobiasFunctionBuilder.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobiasFunctionBuilder.BodyLines;

namespace MobiasFunctionBuilder
{
    public class Operation
    {
        public static ILeftRightable Variable(string name)
        {
            return new OperationVariable(name);
        }

        public static IRightable Constant(object value)
        {
            if (value is OperationConst)
            {
                return value as OperationConst;
            }
            return new OperationConst(value);
        }

        public static IRightable CreateInstance(Type dataType, params IRightable[] variables)
        {
            return new OperationNew(dataType, variables);
        }

        public static IRightable CreateInstance<TData>(params IRightable[] variables)
        {
            return new OperationNew(typeof(TData), variables);
        }

        public static IRightable CreateInstance(Type dataType, IEnumerable<Type> types, params IRightable[] variables)
        {
            var generic = dataType.MakeGenericType(types.ToArray());
            return CreateInstance(generic, variables);
        }

        public static IRightable CreateInstance<TData>(IEnumerable<Type> types, params IRightable[] variables)
        {
            var generic = typeof(TData).MakeGenericType(types.ToArray());
            return CreateInstance(generic, variables);
        }

        public static IBodyLine Invoke(IOperation variable, string methodName, params IOperation[] parameters)
        {
            return new OperationInvoke(variable, methodName, parameters);
        }

        public static IBodyLine Invoke(string variable, string methodName, params IOperation[] parameters)
        {
            return Invoke(Variable(variable), methodName, parameters);
        }

        public static IBodyLine Invoke(Type dataType, string methodName, params IOperation[] parameters)
        {
            return new OperationInvoke(dataType, methodName, parameters);
        }

        public static IBodyLine Invoke<TData>(string methodName, params IOperation[] parameters)
        {
            return Invoke(typeof(TData), methodName, parameters);
        }

        public static IRightable Get(string propertyName)
        {
            return new OperationProperty(propertyName);
        }


    }
}
