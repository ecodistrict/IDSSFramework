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
        decimal min { get; set; }
        [DataMember]
        decimal max { get; set; }
        [DataMember]
        decimal value { get; set; }

        public Number(string label="", string id ="", 
            decimal min=0, decimal max=10, decimal value=5)
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
