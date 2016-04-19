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
    ///  Is a base class to all messaging types that can be sent from the dashboard.
    ///  Is in turn derived from <see cref="IMessage"/>.
    /// </summary>
    [DataContract]
    public class Request : IMessage
    {


    }

}
