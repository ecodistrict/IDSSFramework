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
    /// An input class derived from the class <see cref="NonAtomic"/>. 
    /// </summary>
    /// <remarks>
    /// May be used to define the input specification <see cref="InputSpecification"/>.<br/>
    /// <br/>
    /// In that case it will may be displayed as a list containing <see cref="Atomic"/> and/or <see cref="NonAtomic"/> input.
    /// The user can choose to use one or more.
    /// </remarks>
    [DataContract]
    public class List : NonAtomic
    {
        internal List() { }

        /// <summary>
        /// List constructor.
        /// </summary>
        /// <param name="label">Mandatory label of the visualized component.</param>
        /// <param name="order">Order in which this component should be rendered in the dashboard (ascending order).
        /// Left out or null value will be interpeted as 0 in the dashboard.</param>
        public List(string label, object order = null)
        {
            this.type = "list";
            this.label = label;
            this.order = order;
        }
    }
}
