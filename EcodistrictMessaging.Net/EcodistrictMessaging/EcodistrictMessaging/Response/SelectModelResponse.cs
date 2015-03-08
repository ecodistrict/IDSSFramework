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
    class SelectModelResponse : Response
    {
        [DataMember]
        private string kpiId;
        [DataMember]
        private string variantId;
        [DataMember]
        private InputSpecification inputSpecification;

        public SelectModelResponse(string method, string type, string moduleId, string variantId, string kpiId, InputSpecification inputSpecification)
        {
            this.method = method;
            this.type = type;
            this.moduleId = moduleId;
            this.kpiId = kpiId;
            this.variantId = variantId;
            this.inputSpecification = inputSpecification;

        }
    }
}
