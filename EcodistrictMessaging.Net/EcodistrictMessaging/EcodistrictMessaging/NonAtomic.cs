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
    public class NonAtomic : Input
    {
        [DataMember]
        Dictionary<string, Input> inputs = new Dictionary<string, Input>();

        public void Add(string key, Input item)
        {
            inputs.Add(key, item);
        }
        
    }
}
