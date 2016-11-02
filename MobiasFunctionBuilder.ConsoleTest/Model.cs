using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiasFunctionBuilder.ConsoleTest
{
    public class mDictionary : Dictionary<string, object>
    {
        public void SetVal(string key, object val)
        {
            this[key] = val;
        }
    }

    public class MyObject
    {
        public bool DisplayValue { get; set; }
    }
}
