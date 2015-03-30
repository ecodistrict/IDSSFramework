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
    /// A list of <see cref="Output"/>s. Used by <see cref="ModuleResult"/>.
    /// </summary>
    [DataContract]
    public class Outputs : List<Output>
    {
    }

    /// <summary>
    /// The base class of all outputs that can be sent to the dashboard.
    /// </summary>
    [DataContract]
    public class Output
    {
    }
}
