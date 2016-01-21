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
    public class SelectModuleRequest : Request
    {

    }

    /// <summary>
    /// Derived from the class <see cref="Request"/> and is used as a .Net container for
    /// deseralizing dashboard json-messages of the type "selectModule" request.
    /// This message must be answered with the message type <see cref="SelectModuleResponse"/>.
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
    public class CopyOfSelectModuleRequest : Request
    {

    }
}
