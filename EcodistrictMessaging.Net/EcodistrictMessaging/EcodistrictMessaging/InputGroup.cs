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
            inputs.Add("\"type\"", "inputGroup");
            inputs.Add("\"label\"", label);
        }

        //public override void Add(string key, Input item)
        //{
        //    inputs.Add(key, item);
        //}
    }
}
