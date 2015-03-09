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
    public class Text : Atomic
    {
        public Text(string label, object order = null, object value = null)
        {
            this.type = "text";
            this.label = label;
            this.order = order;
            this.value = value;
        }
    }
}
