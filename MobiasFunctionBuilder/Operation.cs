using MobiasFunctionBuilder.Interface;
using MobiasFunctionBuilder.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
