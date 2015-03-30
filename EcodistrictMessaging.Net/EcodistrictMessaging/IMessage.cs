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
    /// to this type of object by the use of <see cref="Ecodistrict.Messaging.Deseralize.JsonMessage"/>.
    /// 
    /// Messages that will be sent to the dashboard must first be seralized to a json string
    /// this can be done by <see cref="Ecodistrict.Messaging.Seralize.Message"/>.
    /// </remarks> 
    [DataContract]
    public class IMessage 
    {
        [DataMember]
        public string method { get; protected set; }
        [DataMember]
        public string type { get; protected set; }

        /// <summary>
        /// Enum describing the underlying message method; based on the string property method. 
        /// If no valid method can be found this property is set to enum "NoMethod".
        /// </summary>
        private MessageTypes.MMethod eMethod
        {
            get
            {
                switch (method)
                {
                    case "getModules":
                        return MessageTypes.MMethod.GetModules;
                    case "selectModule":
                        return MessageTypes.MMethod.SelectModule;
                    case "startModule":
                        return MessageTypes.MMethod.StartModule;
                    case "ModuleResult":
                        return MessageTypes.MMethod.ModuleResult;
                    default:
                        return MessageTypes.MMethod.NoMethod;
                }
            }
        }

        /// <summary>
        /// Enum describing the underlying message type; based on the string property type. 
        /// If no valid type can be found this property is set to enum "NoType".
        /// </summary>
        private MessageTypes.MType eType
        {
            get
            {
                switch (type)
                {
                    case "request":
                        return MessageTypes.MType.Request;
                    case "response":
                        return MessageTypes.MType.Response;
                    case "result":
                        return MessageTypes.MType.Result;
                    default:
                        return MessageTypes.MType.NoType;
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
                case MessageTypes.MType.Request:
                    switch(eMethod)
                    {
                        case MessageTypes.MMethod.GetModules:
                            return typeof(GetModulesRequest);
                        case MessageTypes.MMethod.SelectModule:
                            return typeof(SelectModuleRequest);
                        case MessageTypes.MMethod.StartModule:
                            return typeof(StartModuleRequest);
                    }
                    break;
                case MessageTypes.MType.Response:
                    switch (eMethod)
                    {
                        case MessageTypes.MMethod.GetModules:
                            return typeof(GetModulesResponse);
                        case MessageTypes.MMethod.SelectModule:
                            return typeof(SelectModuleResponse);
                        case MessageTypes.MMethod.StartModule:
                            return typeof(StartModuleResponse);
                    }
                    break;
                case MessageTypes.MType.Result:
                    switch (eMethod)
                    {
                        case MessageTypes.MMethod.ModuleResult:
                            return typeof(ModuleResult);
                    }
                    break;
            }


            return null;
        }

    }
}
