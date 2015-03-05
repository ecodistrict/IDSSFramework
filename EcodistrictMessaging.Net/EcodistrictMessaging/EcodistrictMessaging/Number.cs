using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecodistrict.Messaging
{
    public class Number : Atomic
    { 
        public Number(string label, object order = null, object value = null,
            object unit = null, object min = null, object max = null)
        {
            inputs.Add("type", "number");
            inputs.Add("label", label);
            
            inputs.Add("order", order);
            inputs.Add("value", value);
            inputs.Add("unit", unit);
            inputs.Add("min", min);
            inputs.Add("max", max);
        }
    }
}
