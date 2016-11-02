using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiasFunctionBuilder.Utils
{
    public class ReflectionUtil
    {
        public static string TypeToString(Type type)
        {
            var typeName = type.Name;
            var thinghy = typeName.IndexOf('`');
            if (thinghy > 0)
            {
                typeName = typeName.Substring(0, thinghy);
            }
            var res = type.Namespace + "." + typeName;
            if (type.IsGenericType)
            {
                res += "<";
                var args = type.GetGenericArguments();
                for (int i = 0; i < args.Length; i++)
                {
                    if (i > 0) res += ", ";
                    res += TypeToString(args[i]);
                }
                res += ">";
            }
            return res;
        }
    }
}
