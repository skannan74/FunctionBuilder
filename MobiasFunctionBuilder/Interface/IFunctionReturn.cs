using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiasFunctionBuilder.Interface
{
    public interface IFunctionReturn : IExpressionResult
    {
        IExpressionResult Returns(string variableName);
    }
}
