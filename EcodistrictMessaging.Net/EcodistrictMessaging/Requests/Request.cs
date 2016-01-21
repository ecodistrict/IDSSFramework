using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Ecodistrict.Messaging.Requests
{
    /// <summary>
    ///  Is a base class to all messaging types that can be sent from the dashboard.
    ///  Is in turn derived from <see cref="IMessage"/>.
    /// </summary>
    [DataContract]
    public class Request : IMessage
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
        /// User ID.
        /// </summary>
        [DataMember]
        public string userId { get; private set; }

        /// <summary>
        /// An indicator on which of the module's kpis that is to be calculated.
        /// </summary>
        [DataMember]
        public string kpiId { get; private set; }

    }

    /// <summary>
    ///  Is a base class to all messaging types that can be sent from the dashboard.
    ///  Is in turn derived from <see cref="IMessage"/>.
    /// </summary>
    [DataContract]
    public class CopyOfRequest : IMessage
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
        /// User ID.
        /// </summary>
        [DataMember]
        public string userId { get; private set; }

        /// <summary>
        /// An indicator on which of the module's kpis that is to be calculated.
        /// </summary>
        [DataMember]
        public string kpiId { get; private set; }

    }
}
