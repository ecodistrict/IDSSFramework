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
    public class ModelResult : Result
    {
        [DataMember]
        private string kpiId;
        [DataMember]
        private string moduleId;
        [DataMember]
        private string variantId;
        [DataMember]
        private List<Output> outputs;

        public ModelResult(string method, string type, string moduleId, string variantId, string kpiId, List<Output> outputs)
        {
            this.method = method;
            this.type = type;
            this.moduleId = moduleId;
            this.kpiId = kpiId;
            this.variantId = variantId;
            this.outputs = outputs;

        }
    }
}
