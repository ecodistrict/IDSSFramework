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
    public class InputSpecification 
    {
        [DataMember]
        List<Input> inputs = new List<Input>();
        
        public void Add(Input item)
        {
            inputs.Add(item);
        }

    }
}
