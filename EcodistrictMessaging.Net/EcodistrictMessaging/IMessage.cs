using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Diagnostics.Contracts;
using System.Threading;

using Ecodistrict.Messaging.Requests;
using Ecodistrict.Messaging.Responses;
using Ecodistrict.Messaging.Results;

namespace Ecodistrict.Messaging
{
    /// <summary> 
    /// Base class to all messaging types that can be sent to/from the dashboard.
    /// </summary> 
    /// <remarks> 
    /// Messages that are sent from the dashboard can be deseralized from a json string
    /// into one of this class derived types by the use of <see cref="Deserialize{T}"/>.<br/>
    /// <br/>
    /// Messages that will be sent to the dashboard must first be serialized into a json-string,
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

    /// <summary> 
    /// Base class to all messaging types that can be sent to/from the dashboard.
    /// </summary>
    /// <remarks> 
    /// Messages that are sent from the dashboard can be deseralized from a json string
    /// into one of this class derived types by the use of <see cref="Deserialize{T}"/>.<br/>
    /// 	<br/>
    /// Messages that will be sent to the dashboard must first be serialized into a json-string,
    /// this can be done by <see cref="Serialize"/>.
    /// </remarks>
    [DataContract]
    [Newtonsoft.Json.JsonConverter(typeof(MessageItemConverter))]
    public class CopyOfIMessage
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

    /// <summary>
    /// A custom converter for deserializing json-strings to the equivocal class <see cref="IMessage"/> to
    /// its derived classes, e.g. <see cref="GetModulesRequest"/> or <see cref="StartModuleResponse"/>.
    /// </summary>
    public class MessageItemConverter : JsonItemConverter<IMessage>//Newtonsoft.Json.Converters.CustomCreationConverter<IMessage>
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object.
        /// </summary>
        /// <param name="objectType">type of object expected, one of the derived classes from <see cref="IMessage"/></param>
        /// <param name="jObject">contents of JSON object that will be deserialized</param>
        /// <returns>An empty derived <see cref="IMessage"/> object that can be filled with the json data.</returns>
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
                            {
                                if (objectType == typeof(Request))  //Makes it possible to only get header-information (i.e. not deserialize the data)
                                    return new Request();
                                else
                                    return new StartModuleRequest();
                            }
                    else if(type == "response")
                            return new StartModuleResponse();
                    break;   
                case "moduleResult":
                    if (type == "result")
                            return new ModuleResult();
                    break;   
            }

            throw new ApplicationException(String.Format("The message type '{0}' or method '{1}' is not supported!", type, method));
        }
    }


}
