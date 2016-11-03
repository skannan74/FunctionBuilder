using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MobiasFunctionBuilder.Utils
{
    public class MethodCallDescriptor
    {
        public MethodCallDescriptor()
        {
            ParamTypes = new List<Type>();
            ParamValues = new List<object>();
            GoodFrom = 0;
        }

        public MethodBase Method { get; set; }
        public List<Type> ParamTypes { get; set; }
        public List<object> ParamValues { get; set; }
        public int GoodFrom { get; set; }
    }
}
