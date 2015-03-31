using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Ecodistrict.Messaging
{
    /// <summary>
    /// Derived from the class <see cref="Response"/> and is a .Net container that can be seralized 
    /// to a json-message that can be sent to the dashboard as a response to the "startModule" request
    /// <see cref=" StartModuleRequest"/>.
    /// </summary>
    /// <remarks>
    /// For a "startModule" request the dashboard wants 2 responses.
    /// 1. When the Module starts computing the output data: Response with status="processing"
    /// 2. When the Module have succesfully finished computing the output data: Response with status="success"
    /// </remarks>
    [DataContract]
    public class StartModuleResponse : Response
    {
        /// <summary>
        /// The kpi id that this response refers to.
        /// </summary>
        [DataMember]
        protected string kpiId;

        /// <summary>
        /// The variant id aquired from the dashboard in the <see cref="StartModuleRequest"/> message.
        /// </summary>
        [DataMember]
        protected string variantId;

        /// <summary>
        /// The progress of the calculation of the kpi.
        /// </summary>
        [DataMember]
        protected string status;

        internal StartModuleResponse() { }

        /// <summary>
        /// Defines the response to the "startModule" request sent by the dashboard.
        /// Can be seralized to a json-string that can be interpeted by the dashboard.
        /// </summary>
        /// <param name="moduleId">Unique identifer of the module using the messaging protokoll.</param>
        /// <param name="variantId">Used by dashboard for tracking.</param>
        /// <param name="kpiId">The kpi that the dashboard previously selected.</param>
        /// <param name="status">Status message</param>
        public StartModuleResponse(string moduleId, string variantId, string kpiId, MessageTypes.ModuleStatus status)
        {
            this.method = "startModule";
            this.type = "response";
            this.moduleId = moduleId;
            this.variantId = variantId;
            this.kpiId = kpiId;

            if (status == MessageTypes.ModuleStatus.Processing)
                this.status = "processing";
            else if (status == MessageTypes.ModuleStatus.Success)
                this.status = "success";
        }
    }
}
