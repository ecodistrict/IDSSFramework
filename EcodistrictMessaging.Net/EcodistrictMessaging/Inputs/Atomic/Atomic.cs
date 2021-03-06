﻿using System;
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
        /// Is <see cref="Boolean">false</see> if <see cref="value"/> is omitted in the constructor.
        /// </remarks>
        /// <returns> 
        /// <see cref="Boolean">true</see> if the class property <see cref="value"/>  should be serialized with the <see cref="Input"/> object.
        /// </returns>
        public bool ShouldSerializevalue()
        {
            return value != null;
        }

        /// <summary>
        /// Returns the value of this <see cref="Atomic"/> <see cref="Input"/> type.
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            return value;
        }
    }
}
