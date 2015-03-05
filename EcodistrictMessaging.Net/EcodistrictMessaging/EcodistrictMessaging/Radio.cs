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

        public Radio(string label)
        {
            inputs.Add("type", "radio");
            inputs.Add("label", label);
        }
    }
}
