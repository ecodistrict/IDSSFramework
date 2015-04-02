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
    /// deseralizing dashboard json-messages specifying the mehod "getModules" and the 
    /// type "request".<br/>
    /// <br/>
    /// This message should be answered with a message of the type <see cref="GetModulesResponse"/>.
    /// </summary>
    [DataContract]
    public class GetModulesRequest : Request
    {
    }
}
