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
    class Text : Atomic
    {
        //TODO Text data
        //[DataContract]

        public Text(string label, string id)
        {
            this.type = "tumber";
            this.label = label;
            this.id = id;
        }
    }
}
