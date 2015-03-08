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
    public class StartModelRequest : Request
    {
        [DataMember]
        public string moduleId { get; private set; }
        [DataMember]
        public string variantId { get; private set; }
        [DataMember]
        public string kpiId { get; private set; }
        [DataMember]
        public Dictionary<string, object> inputData { get; private set; }  //TODO inputData
    }
}
