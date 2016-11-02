using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiasFunctionBuilder.Interface
{
    public interface IFunctionParameter
    {
        IBodyOrParameter InputParameter(Type type, string name);
        IBodyOrParameter InputParameter<TData>(string name);
    }
}
