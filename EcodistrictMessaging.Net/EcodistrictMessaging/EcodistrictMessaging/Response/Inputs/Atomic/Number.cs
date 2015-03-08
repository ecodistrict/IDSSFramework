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
        object unit;
        [DataMember]
        object min;
        [DataMember]
        object max;

        public Number(string label, object order = null, object value = null,
            object unit = null, object min = null, object max = null)
        {
            this.type = "number";
            this.label = label;
            this.order = order;
            this.value = value;
            this.unit = unit;
            this.min = min;
            this.max = max;
        }

        public bool ShouldSerializeunit()
        {
            return unit != null;
        }

        public bool ShouldSerializemin()
        {
            return min != null;
        }

        public bool ShouldSerializemax()
        {
            return max != null;
        }

    }
}
