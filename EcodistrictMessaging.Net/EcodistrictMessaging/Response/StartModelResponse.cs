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
    /// to a json-message that can be sent to the dashboard as a response to the "startModel" request
    /// <see cref=" StartModelRequest"/>.
    /// </summary>
    /// <remarks>
    /// For a "startModel" request the dashboard wants 2 responses.
    /// 1. When the model starts computing the output data: Response with status="processing"
    /// 2. When the model have succesfully finished computing the output data: Response with status="success"
    /// </remarks>
    [DataContract]
    public class StartModelResponse : Response
    {
        [DataMember]
        protected string variantId;
        [DataMember]
        protected string kpiId;
        [DataMember]
        protected string status;

        /// <summary>
        /// Defines the response to the "startModel" request sent by the dashboard.
        /// Can be seralized to a json-string that can be interpeted by the dashboard.
        /// </summary>
        /// <param name="moduleId">Unique identifer of the module using the messaging protokoll.</param>
        /// <param name="variantId">Used by dashboard for tracking.</param>
        /// <param name="kpiId">The kpi that the dashboard previously selected.</param>
        /// <param name="status">Staus message</param>
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
