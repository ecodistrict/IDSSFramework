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
    class Checkbox : Atomic
    {
        //TODO Checkbox data
        //[DataContract]

        public Checkbox(string label, string id)
        {
            this.type = "checkbox";
            this.label = label;
            this.id = id;
        }
    }
}
