using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiasFunctionBuilder.Interface
{
    public interface IWhile
    {
        IBodyLine Do(IBodyLine firstCodeLine, params IBodyLine[] codeLines);
    }
}
