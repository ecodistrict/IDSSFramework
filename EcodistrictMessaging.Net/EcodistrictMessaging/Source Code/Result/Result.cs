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
    /// The base class of all result messages that can be sent to the dasboard.
    /// </summary>
    /// <remarks>
    /// Currently only one type of result message is defined, see <see cref="ModuleResult"/>.
    /// </remarks>
    [DataContract]
    public class Result : IMessage
    {

    }
}
