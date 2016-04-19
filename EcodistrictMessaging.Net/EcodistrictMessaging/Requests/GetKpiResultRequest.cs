using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Ecodistrict.Messaging.Requests
{
    [DataContract]
    public class GetKpiResultRequest : Request
    {
        /// <summary>
        /// The unique identifier of the module that the dashboard want start the calculation procedure.
        /// </summary>
        /// <remarks>
        /// If this id is not equal to the current module's id, the module can ignore this message.
        /// </remarks>
        [DataMember]
        public string moduleId { get; protected set; }

        /// <summary>
        /// Used by dashboard for tracking. The same variantId that is given by the dashboard 
        /// should be returned by the <see cref="StartModuleResponse"/>.
        /// </summary>
        [DataMember]
        public string variantId { get; protected set; }

        [DataMember]
        public string caseId { get; protected set; }

        /// <summary>
        /// User ID.
        /// </summary>
        [DataMember]
        public string userId { get; protected set; }

        /// <summary>
        /// An indicator on which of the module's kpis that is to be calculated.
        /// </summary>
        [DataMember]
        public string kpiId { get; protected set; }
        
        internal GetKpiResultRequest() { }

        public GetKpiResultRequest(string moduleId, string caseId, string variantId, string kpiId)
        {
            this.method = "setKpiResult";
            this.type = "request";
            this.moduleId = moduleId;
            this.caseId = caseId;
            this.variantId = variantId;
            this.kpiId = kpiId;
        }
    }
}
