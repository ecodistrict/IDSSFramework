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
    class Select : Atomic
    {
        //TODO Select data
        //[DataContract]

        public Select(string label)
        {
            inputs.Add("type", "select");
            inputs.Add("label", label);
        }
    }
}
