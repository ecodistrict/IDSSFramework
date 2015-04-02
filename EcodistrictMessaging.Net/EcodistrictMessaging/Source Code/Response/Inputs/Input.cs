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
        /// <summary>
        /// Mandatory label, describing the component.
        /// </summary>
        /// <remarks>
        /// The label will be visualized next to the input selector described by this combonent. 
        /// E.g. <see cref="Number"/>, <see cref="Checkbox"/>, <see cref="List"/>, ...
        /// </remarks>
        [DataMember]
        protected string label;

        /// <summary>
        /// What typ of component the dashboard shall visualize, e.g. "number","text",'list',...
        /// </summary>
        [DataMember]
        protected string type;

        /// <summary>
        /// The order in which this component should be rendered in the dashboard (ascending order).
        /// Left out or null value will be interpeted as 0 in the dashboard.
        /// </summary>
        [DataMember]
        protected object order;

        /// <summary>
        /// If the class property "order" should be serialized with the Input object.
        /// </summary>
        /// <remarks>
        /// Is <b>false</b> if <see cref="order"/> is omitted in the constructor.
        /// </remarks>
        /// <returns> 
        /// <b>true</b> if the class property <see cref="order"/>  should be serialized with the <see cref="Input"/> object.
        /// </returns>
        public bool ShouldSerializeorder()
        {
            return order != null;
        }
    }
}
