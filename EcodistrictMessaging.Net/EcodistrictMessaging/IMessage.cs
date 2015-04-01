using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using System.Diagnostics.Contracts;
using System.Threading;

namespace Ecodistrict.Messaging
{
    /// <summary> 
    /// Base class to all messagingtypes that can be sent to/from the dashboard.
    /// </summary> 
    /// <remarks> 
    /// Messages that are sent from the dashboard can be deseralized from a json string
    /// to this type of object by the use of <see cref="Deserialize"/>.<br/>
    /// <br/>
    /// Messages that will be sent to the dashboard must first be seralized to a json string
    /// this can be done by <see cref="Serialize"/>.
    /// </remarks> 
    [DataContract]
    [Newtonsoft.Json.JsonConverter(typeof(MessageItemConverter))]
    public class IMessage 
    {
        /// <summary>
        /// String representation of the method.
        /// </summary>
        [DataMember]
        public string method { get; protected set; }
        
        /// <summary>
        /// String representation of the type.
        /// </summary>
        [DataMember]
        public string type { get; protected set; }
    }

    public class MessageItemConverter : JsonItemConverter<IMessage>//Newtonsoft.Json.Converters.CustomCreationConverter<IMessage>
    {
        public override IMessage Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        protected override IMessage Create(Type objectType, Newtonsoft.Json.Linq.JObject jObject)
        {
            var type = (string)jObject.Property("type");
            var method = (string)jObject.Property("method");

            switch (method)
            {
                case "getModules":
                    if(type == "request")
                            return new GetModulesRequest();
                    else if(type == "response")
                            return new GetModulesResponse();
                    break;                    
                case "selectModule":
                    if(type == "request")
                            return new SelectModuleRequest();
                    else if(type == "response")
                            return new SelectModuleResponse();
                    break;   
                case "startModule":
                    if(type == "request")
                            return new StartModuleRequest();
                    else if(type == "response")
                            return new StartModuleResponse();
                    break;   
                case "ModuleResult":
                    if (type == "result")
                            return new ModuleResult();
                    break;   
            }

            throw new ApplicationException(String.Format("The message type '{0}' or method '{1}' is not supported!", type, method));
        }
    }


}
