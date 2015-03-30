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
    /// deseralizing dashboard json-messages of the type "getModules" request.
    /// 
    /// This message must be answered with the message type <see cref="GetModulesResponse"/>.
    /// </summary>
    [DataContract]
    public class GetModulesRequest : Request
    {
    }
}
