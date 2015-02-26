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
    class Radio : Atomic
    {
        //TODO Radio data
        //[DataContract]

        public Radio(string label, string id)
        {
            this.type = "radio";
            this.label = label;
            this.id = id;
        }
    }
}
