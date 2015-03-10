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
    public class StartModelResponse : Response
    {
        [DataMember]
        private string variantId;
        [DataMember]
        private string kpiId;
        [DataMember]
        private string status;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleId">Unique identifer of the module using the messaging protokoll.</param>
        /// <param name="variantId">Used by dashboard for tracking.</param>
        /// <param name="kpiId">The kpi that the dashboard previously selected.</param>
        /// <param name="status"></param>
        public StartModelResponse(string moduleId, string variantId, string kpiId, string status)
        {
            this.method = "startModel";
            this.type = "response";
            this.moduleId = moduleId;
            this.variantId = variantId;
            this.kpiId = kpiId;
            this.status = status;
        }
    }
}
