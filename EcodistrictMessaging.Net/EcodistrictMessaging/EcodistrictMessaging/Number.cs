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

        public Number(string label, string id,
            object min = null, object max = null, object value = null)
        {
            this.type = "number";
            this.label = label;
            this.id = id;

            this.min = min;
            this.max = max;
            this.value = value;
        }

    }
}
