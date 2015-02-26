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
    class InputGroup : NonAtomic
    {
        public InputGroup(string label, string id)
        {
            this.type = "inputGroup";
            this.label = label;
            this.id = id;
        }

        public override void Add(Input item)
        {
            inputs.Add(item);
        }
    }
}
