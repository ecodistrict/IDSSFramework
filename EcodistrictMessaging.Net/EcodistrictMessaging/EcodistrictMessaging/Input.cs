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
    [KnownType(typeof(Atomic))]
    public class Input
    {
        [DataMember]
        protected string type { get; set; }
        [DataMember]
        protected string label { get; set; }
        [DataMember]
        protected string id { get; set; }
    }
}
