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
    /// Derived from the class <see cref="Request"/> and is used as a .Net container for
    /// deseralizing dashboard json-messages of the type "startModule" request.
    /// 
    /// This message must be answered with the message type <see cref="StartModuleResponse"/>.
    /// </summary>
    [DataContract]
    public class StartModuleRequest : Request
    {
        /// <summary>
        /// The unique identifier of the module that the dashboard want start the calculation procedure.
        /// </summary>
        /// <remarks>
        /// If this id is not equal to the current module's id, the module can ignore this message.
        /// </remarks>
        [DataMember]
        public string moduleId { get; private set; }

        /// <summary>
        /// Used by dashboard for tracking. The same variantId that is given by the dashboard 
        /// should be returned by the <see cref="StartModuleResponse"/>.
        /// </summary>
        [DataMember]
        public string variantId { get; private set; }

        /// <summary>
        /// An indicator on which of the module's kpis that is to be calculated.
        /// </summary>
        [DataMember]
        public string kpiId { get; private set; }

        /// <summary>
        /// The data the module need to calculate the selected kpi.
        /// </summary>
        [DataMember]
        public Dictionary<string, object> inputData { get; private set; } 
    }
}
