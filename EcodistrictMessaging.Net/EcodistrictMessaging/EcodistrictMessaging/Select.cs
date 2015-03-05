using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecodistrict.Messaging
{
    public class Select : Atomic
    {

        public Select(string label, Options options, object order = null, object value = null)
        {
            inputs.Add("type", "select");
            inputs.Add("label", label);
            
            inputs.Add("order", order);
            //if (value!=null)
            //    inputs.Add("value", value);
            //else
                inputs.Add("value", value);
            inputs.Add("options", options);
        }
    }
}
