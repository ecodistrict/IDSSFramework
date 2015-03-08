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
    class StartModelResponse : Response
    {
        [DataMember]
        private string variantId;
        [DataMember]
        private string kpiId;
        [DataMember]
        private string status;

        public StartModelResponse(string method, string type, string moduleId, string variantId, string kpiId, string status)
        {
            this.method = method;
            this.type = type;
            this.moduleId = moduleId;
            this.variantId = variantId;
            this.kpiId = kpiId;
            this.status = status;
        }
    }
}
