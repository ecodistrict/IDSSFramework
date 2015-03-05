using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecodistrict.Messaging
{
    public class Text : Atomic
    {
        public Text(string label, object order = null, object value = null)
        {
            inputs.Add("type", "text");
            inputs.Add("label", label);

            inputs.Add("order", order);
            inputs.Add("value", value);
        }
    }
}
