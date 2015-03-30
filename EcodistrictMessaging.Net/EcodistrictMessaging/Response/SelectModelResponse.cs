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
    /// to a json-message that can be sent to the dashboard as a response to the "selectModule" request
    /// <see cref=" SelectModuleRequest"/>.
    /// </summary>
    [DataContract]
    public class SelectModuleResponse : Response
    {
        [DataMember]
        protected string kpiId;
        [DataMember]
        protected string variantId;
        [DataMember]
        protected InputSpecification inputSpecification;

        /// <summary>
        /// Defines the response to the "selectModule" request sent by the dashboard.
        /// Can be seralized to a json-string that can be interpeted by the dashboard.
        /// </summary>
        /// <param name="moduleId">Unique identifer of the module using the messaging protokoll.</param>
        /// <param name="variantId">Used by dashboard for tracking.</param>
        /// <param name="kpiId">The kpi that the dashboard previously selected.</param>
        /// <param name="inputSpecification">The specification to the dashboard on what data the Module need.</param>
        public SelectModuleResponse(string moduleId, string variantId, string kpiId, InputSpecification inputSpecification)
        {
            this.method = "selectModule";
            this.type = "response";
            this.moduleId = moduleId;
            this.kpiId = kpiId;
            this.variantId = variantId;
            this.inputSpecification = inputSpecification;

        }
    }
}
