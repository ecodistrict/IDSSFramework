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
        protected string kpiId;
        [DataMember]
        protected string moduleId;
        [DataMember]
        protected string variantId;
        [DataMember]
        protected Outputs outputs;

        public ModelResult(string moduleId, string variantId, string kpiId, Outputs outputs)
        {
            this.method = "modelResult";
            this.type = "result";
            this.moduleId = moduleId;
            this.kpiId = kpiId;
            this.variantId = variantId;
            this.outputs = outputs;

        }
    }
}
