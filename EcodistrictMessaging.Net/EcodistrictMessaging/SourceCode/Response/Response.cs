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
    ///  Is a base class to all messagingtypes that can be sent to the dashboard.
    /// </summary>
    [DataContract]
    public class Response : IMessage
    {
        /// <summary>
        /// The unique identifier of the module that the dashboard can use to target calculations on a specific module.
        /// </summary>
        /// <remarks>
        /// Must be a web-friendly string <see href="https://github.com/ecodistrict/IDSSFramework/wiki/Messaging-reference#web-friendly-strings"/>.
        /// </remarks>
        [DataMember]
        protected string moduleId;
    }
}
