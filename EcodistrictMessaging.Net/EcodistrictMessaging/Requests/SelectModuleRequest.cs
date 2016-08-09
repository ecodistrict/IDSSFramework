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
    /// Derived from the class <see cref="Request"/> and is used as a .Net container for
    /// deseralizing dashboard json-messages of the type "selectModule" request.
    /// 
    /// This message must be answered with the message type <see cref="SelectModuleResponse"/>.
    /// 
    /// Depricated the dashboard doesn't send this message anymore.
    /// </summary>
    /// <example>
    /// A simple reconstruction of a dashboard json message. Normaly these are acquired through
    /// an IMB-hub in an byte array, in that case you may use Deserialize.JsonByteArr(bArr). 
    /// See <see cref="Deserialize{T}"/>.
    /// <code>
    /// //json-string from dashboard
    /// string message = "{" + 
    ///                   "\"type\": \"request\"," +
    ///                   "\"method\": \"selectModule\"," +
    ///                   "\"moduleId\": \"foo-bar_cheese-Module-v1-0\"," +
    ///                   "\"variantId\": \"503f191e8fcc19729de860ea\"," +
    ///                   "\"kpiId\": \"cheese-taste-kpi\"" +
    ///                  "}";
    /// ///Message reconstructed into a .Net object.
    /// IMessage recievedMessage = Deserialize.JsonString(message);
    /// //Write object type to console
    /// Console.WriteLine(recievedMessage.GetType().ToString());
    /// //Output: Ecodistrict.Messaging.SelectModuleRequest
    /// </code>
    /// </example>
    /// <seealso cref="IMessage"/>
    /// <seealso cref="SelectModuleResponse"/>
    /// <seealso cref="Deserialize{T}"/>
    [DataContract]
    public class SelectModuleRequest : Request  //TODO Is not used anymore, remove when the messaging format has been finalized.
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
        /// An indicator on which of the module's kpis that is to be calculated.
        /// </summary>
        [DataMember]
        public string kpiId { get; protected set; }
        
    }

}
