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
    /// The base class for all types of inputs that can be used in the input specification
    /// <see cref="InputSpecification"/>.
    /// </summary>
    [DataContract]
    public class Input
    {
        [DataMember]
        protected string label;
        [DataMember]
        protected string type;
        [DataMember]
        protected object order;

        /// <summary>
        /// If the class property "order" should be serialized with the Input object.
        /// </summary>
        /// <returns>If the class property "order" should be serialized with the Input object.</returns>
        public bool ShouldSerializeorder()
        {
            return order != null;
        }
    }
}
