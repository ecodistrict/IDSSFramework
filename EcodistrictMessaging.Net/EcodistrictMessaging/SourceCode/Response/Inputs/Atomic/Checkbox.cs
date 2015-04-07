using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using System.Runtime.InteropServices;

namespace Ecodistrict.Messaging
{
    /// <summary>
    /// An input class derived from the class <see cref="Atomic"/>.
    /// May be used to define the input specification <see cref="InputSpecification"/>.
    /// </summary>
    /// <remarks>
    /// May be used to define the input specification <see cref="InputSpecification"/>.
    /// 
    /// In that case it will be visualized as a checkbox and will indicate whether the 
    /// property defined by this object should be used or not.
    /// </remarks>
    [DataContract]
    public class Checkbox : Atomic
    {

        internal Checkbox() { }

        /// <summary>
        /// Checkbox constructor.
        /// </summary>
        /// <param name="label">Mandatory label of the visualized component, e.g. "Use insulation".</param>
        /// <param name="order">Order in which this component should be rendered in the dashboard (ascending order).
        /// Left out or null value will be interpeted as 0 in the dashboard.</param>
        /// <param name="value">Initial value of the checkbox.</param>
        public Checkbox(string label, int? order = null, bool value = false)
        {
            this.type = "checkbox";
            this.label = label;
            this.order = order;
            this.value = value;
        }
    }
}
