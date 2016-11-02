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
            var fn = Sample.BuildLambda();

            Sample.mDictionary result = fn.Compile().DynamicInvoke() as Sample.mDictionary;
        }
    }
}
