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

        /// <summary>
        /// Enum describing the underlying message method; based on the string property method. 
        /// If no valid method can be found this property is set to enum "NoMethod".
        /// </summary>
        private MessageTypes.MessageMethod eMethod
        {
            get
            {
                switch (method)
                {
                    case "getModules":
                        return MessageTypes.MessageMethod.GetModules;
                    case "selectModule":
                        return MessageTypes.MessageMethod.SelectModule;
                    case "startModule":
                        return MessageTypes.MessageMethod.StartModule;
                    case "ModuleResult":
                        return MessageTypes.MessageMethod.ModuleResult;
                    default:
                        return MessageTypes.MessageMethod.NoMethod;
                }
            }
        }

        /// <summary>
        /// Enum describing the underlying message type; based on the string property type. 
        /// If no valid type can be found this property is set to enum "NoType".
        /// </summary>
        private MessageTypes.MessageType eType
        {
            get
            {
                switch (type)
                {
                    case "request":
                        return MessageTypes.MessageType.Request;
                    case "response":
                        return MessageTypes.MessageType.Response;
                    case "result":
                        return MessageTypes.MessageType.Result;
                    default:
                        return MessageTypes.MessageType.NoType;
                }
            }
        }

        /// <summary>
        /// Gets the underlying derived class based on method and type property.
        /// </summary>
        /// <returns>Derived type</returns>
        public Type GetDerivedType()
        {

            switch(eType)
            {
                case MessageTypes.MessageType.Request:
                    switch(eMethod)
                    {
                        case MessageTypes.MessageMethod.GetModules:
                            return typeof(GetModulesRequest);
                        case MessageTypes.MessageMethod.SelectModule:
                            return typeof(SelectModuleRequest);
                        case MessageTypes.MessageMethod.StartModule:
                            return typeof(StartModuleRequest);
                    }
                    break;
                case MessageTypes.MessageType.Response:
                    switch (eMethod)
                    {
                        case MessageTypes.MessageMethod.GetModules:
                            return typeof(GetModulesResponse);
                        case MessageTypes.MessageMethod.SelectModule:
                            return typeof(SelectModuleResponse);
                        case MessageTypes.MessageMethod.StartModule:
                            return typeof(StartModuleResponse);
                    }
                    break;
                case MessageTypes.MessageType.Result:
                    switch (eMethod)
                    {
                        case MessageTypes.MessageMethod.ModuleResult:
                            return typeof(ModuleResult);
                    }
                    break;
            }


            return null;
        }

    }
}
