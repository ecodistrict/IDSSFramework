using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Ecodistrict.Messaging
{
    [DataContract]
    public class Number : Atomic
    { 
        [DataMember]
        object min { get; set; }
        [DataMember]
        object max { get; set; }
        [DataMember]
        object value { get; set; }

        public Number(string label,
            object min = null, object max = null, object value = null)
        {
            inputs.Add("type", "number");
            inputs.Add("label", label);

            inputs.Add("min", min);
            inputs.Add("max", max);
            inputs.Add("value", value);
        }
    }
}
