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
    /// The base class for all atomic types of inputs that can be used in the <see cref="InputSpecification"/>.
    /// Derived from the general class <see cref="Input"/>.
    /// </summary>
    /// <remarks>
    /// A atomic input will be derived in the dashboard into some simple object like choose a number 
    /// or a checkbox indicating whether a property should be used or not.
    /// </remarks>
    [DataContract]
    public class Atomic : Input
    {
        [DataMember]
        protected object value;

        public bool ShouldSerializevalue()
        {
            return value != null;
        }
    }
}
