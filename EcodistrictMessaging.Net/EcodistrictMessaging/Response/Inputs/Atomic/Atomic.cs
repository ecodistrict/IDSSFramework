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
    /// An atomic input will be derived in the dashboard into some simple object, e.g. choose a number 
    /// or a checkbox indicating whether a property should be used or not.
    /// </remarks>
    [DataContract]
    public class Atomic : Input
    {
        /// <summary>
        /// The initial value of the <see cref="Atomic"/> input.
        /// </summary>
        [DataMember]
        protected object value;


        /// <summary>
        /// Indicator whether <see cref="Serialize"/> should serialize the property <see cref="value"/>.
        /// </summary>
        /// <remarks>
        /// Is false if <see cref="value"/> is ommitted in the constructor.
        /// </remarks>
        public bool ShouldSerializevalue()
        {
            return value != null;
        }
    }
}
