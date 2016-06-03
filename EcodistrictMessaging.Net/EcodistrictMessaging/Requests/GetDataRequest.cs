using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Ecodistrict.Messaging.Requests
{
    /// <summary>
    /// Request data from data module
    /// </summary>
    [DataContract]
    public class GetDataRequest : Request
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

        [DataMember]
        public string userId { get; protected set; }
                
        [DataMember]
        public string calculationId { get; protected set; }

        [DataMember]
        public string eventId { get; protected set; }

        [DataMember]
        public object options { get; protected set; }

        internal GetDataRequest() { }

        public GetDataRequest(string moduleId, string calculationId, string caseId, string variantId, string eventId, string userId = null)
        {
            this.method = "getData";
            this.type = "request";
            this.moduleId = moduleId;
            this.calculationId = calculationId;
            this.caseId = caseId;
            this.variantId = variantId;
            this.eventId = eventId;
            this.userId = userId;
        }

        public GetDataRequest(StartModuleRequest smr, string calculationId, string eventId)
        {
            this.method = "getData";
            this.type = "request";
            this.moduleId = smr.moduleId;
            this.calculationId = calculationId;
            this.caseId = smr.caseId;
            this.variantId = smr.variantId;
            this.userId = smr.userId;
            this.eventId = eventId;
        }
    }
}
