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
    public class Select : Atomic
    {
        [DataMember]
        Options options;

        public Select(string label, Options options, int? order = null, object value = null)
        {
            this.type = "select";
            this.label = label;
            this.order = order;
            this.value = value;
            this.options = options;
        }
    }
}
