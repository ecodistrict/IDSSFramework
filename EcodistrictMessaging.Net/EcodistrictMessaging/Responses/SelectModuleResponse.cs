using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Ecodistrict.Messaging.Responses
{
    /// <summary>
    /// Derived from the class <see cref="Response"/> and is a .Net container that can be seralized 
    /// to a json-message that can be sent to the dashboard as a response to the "selectModule" request
    /// <see cref=" SelectModuleRequest"/>.
    /// 
    /// Deprecated the dashboard does not send <see cref=" SelectModuleRequest"/> anymore.
    /// </summary>
    ///  <example>
    /// An example of a response created when the application received a <see cref="SelectModuleRequest"/> from the 
    /// dashboard. The response message should be sent trough a IMB-hub as a <see cref="T:byte[]"/> originated from 
    /// a json-string. <br/>
    /// <br/>
    /// However, in this example we demonstrates the usage of the .Net message-type <see cref="SelectModuleResponse"/>
    /// and how it can be serialized into a valid json-string that can be interpreted by the dashboard.
    /// <code>
    /// //This module's id
    /// string moduleId = "foo-bar_cheese-Module-v1-0";
    ///
    /// //The dashboard message received (as a json-string)
    /// string message = "{" +
    ///                    "\"type\": \"request\"," +
    ///                    "\"method\": \"selectModule\"," +
    ///                    "\"moduleId\": \"foo-bar_cheese-Module-v1-0\"," +
    ///                    "\"variantId\": \"503f191e8fcc19729de860ea\"," +
    ///                    "\"kpiId\": \"cheese-taste-kpi\"" +
    ///                  "}";
    /// //Message reconstructed into a .Net object.
    /// SelectModuleRequest recievedMessage = (SelectModuleRequest)Deserialize.JsonString(message);
    ///
    ///    //Create the IMessage response.
    ///    SelectModuleResponse mResponse = new SelectModuleResponse(
    ///        moduleId: recievedMessage.moduleId,
    ///        variantId: recievedMessage.variantId,
    ///        kpiId: recievedMessage.kpiId);
    ///
    ///    //json-string that can be interpreted by the dashboard
    ///    //In this case indented in order to be easier to read (won't effect the dashboard). 
    ///    string messageResponse = Serialize.ToJsonString(mResponse, true);
    ///
    ///    //Write the message to the console
    ///    Console.WriteLine(messageResponse);
    ///
    ///    //Output:
    ///    //{
    ///    //  "kpiId": "cheese-taste-kpi",
    ///    //  "variantId": "503f191e8fcc19729de860ea",
    ///    //  "moduleId": "foo-bar_cheese-Module-v1-0",
    ///    //  "method": "selectModule",
    ///    //  "type": "response"
    ///    //}
    ///
    ///}
    /// </code>
    /// </example>
    /// <seealso cref="IMessage"/>
    /// <seealso cref="SelectModuleRequest"/>
    /// <seealso cref="Serialize"/>
    [DataContract]
    public class SelectModuleResponse : Response   //TODO Is not used anymore, remove when the messaging format has been finalized.
    {
        /// <summary>
        /// The kpi id that this response refers to.
        /// </summary>
        [DataMember]
        protected string kpiId;

        [DataMember]
        public string caseId { get; private set; }

        /// <summary>
        /// The variant id acquired from the dashboard in the <see cref="SelectModuleRequest"/> message.
        /// </summary>
        [DataMember]
        protected string variantId;
        
        internal SelectModuleResponse() { }

        /// <summary>
        /// Defines the response to the "selectModule" request sent by the dashboard.
        /// Can be serialized to a json-string that can be interpreted by the dashboard.
        /// </summary>
        /// <param name="moduleId">Unique identifer of the module using the messaging protokoll.</param>
        /// <param name="variantId">Used by dashboard for tracking.</param>
        /// <param name="kpiId">The kpi that the dashboard previously selected.</param>
        public SelectModuleResponse(string moduleId, string variantId, string caseId, string kpiId)
        {
            this.method = "selectModule";
            this.type = "response";
            this.moduleId = moduleId;
            this.kpiId = kpiId;
            this.variantId = variantId;
            this.caseId = caseId;
        }
    }
}
