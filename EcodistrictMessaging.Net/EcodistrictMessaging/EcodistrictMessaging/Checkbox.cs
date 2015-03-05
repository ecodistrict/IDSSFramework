using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Ecodistrict.Messaging
{
    class Checkbox : Atomic
    {
        //TODO Checkbox data

        public Checkbox(string label, object order = null, object value = null)
        {
            inputs.Add("type", "checkbox");
            inputs.Add("label", label);

            inputs.Add("order", order);
            inputs.Add("value", value);
        }
    }
}
