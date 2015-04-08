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
    /// The base class for all non-atomic types of inputs that can be used in the <see cref="InputSpecification"/>.
    /// Derived from the general class <see cref="Input"/>.
    /// </summary>
    /// <remarks>
    /// The non-atomic input types may contain other <see cref="Input"/>s (<see cref="Atomic"/> and <see cref="NonAtomic"/>).
    /// </remarks>
    [DataContract]
    public class NonAtomic : Input
    {
        /// <summary>
        /// The collection of <see cref="Input"/>s, in this <see cref="NonAtomic"/> input.
        /// </summary>
        [DataMember]
        protected Dictionary<string, Input> inputs = new Dictionary<string, Input>();

        /// <summary>
        /// The common property of all <see cref="NonAtomic"/> <message /> <see cref="Input"/>s, is that they can contain other
        /// inputs (atomic and non-atomic types).
        /// </summary>
        /// <param name="key">Unique key at this level. E.g. a <see cref="List"/> may only contain <see cref="Input"/>s
        /// with different keyes but it may contain a list were a key is repeated.
        /// </param>
        /// <param name="item"></param>
        public void Add(string key, Input item)
        {
            inputs.Add(key, item);
        }
        
    }
}
