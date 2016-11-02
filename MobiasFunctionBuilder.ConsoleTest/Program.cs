using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiasFunctionBuilder.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var fn = Sample.BuildLambdaAndReturnDictionaryExpression();

            mDictionary result = fn.Compile().DynamicInvoke() as mDictionary;
        }
    }
}
