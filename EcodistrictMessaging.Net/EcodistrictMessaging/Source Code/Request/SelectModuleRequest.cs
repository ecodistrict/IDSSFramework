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
    /// deseralizing dashboard json-messages of the type "selectModule" request.
    /// 
    /// This message must be answered with the message type <see cref="SelectModuleResponse"/>.
    /// </summary>
    /// <example>
    /// A simple reconstruction of a dashboard json message. Normaly these are acquired through
    /// an IMB-hub in an byte array, in that case you may use Deserialize.JsonByteArr(bArr). 
    /// See <see cref="Deserialize"/>.
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
    /// <seealso cref="Deserialize"/>
    /// <seealso cref="SelectModuleResponse"/>
    [DataContract]
    public class SelectModuleRequest : Request
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
        /// should be returned by the <see cref="SelectModuleResponse"/>.
        /// </summary>
        [DataMember]        
        public string variantId { get; private set; }
        
        /// <summary>
        /// An indicator on which of the module's KPIs <see cref="InputSpecification"/> that is to be returned.
        /// </summary>
        /// <remarks>
        /// The <see cref="InputSpecification"/> is returned trough the <see cref="SelectModuleResponse"/> message.
        /// </remarks>
        [DataMember]
        public string kpiId { get; private set; }
    }
}
