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
    /// An input class derived from the class <see cref="Atomic"/>. 
    /// </summary>
    /// <remarks>
    /// May be used to define the input specification <see cref="InputSpecification"/>.
    /// 
    /// In that case it will may be displayed as an input box were the user may
    /// supply a text.
    /// </remarks>
    [DataContract]
    public class Text : Atomic
    {
        internal Text() { }

        /// <summary>
        /// Text constructor.
        /// </summary>
        /// <param name="label">Mandatory label of the visualized component.</param>
        /// <param name="order">Order in which this component should be rendered in the dashboard (ascending order).
        /// Left out or null value will be interpeted as 0 in the dashboard. </param>
        /// <param name="value">Default text value</param>
        public Text(string label, int? order = null, object value = null)
        {
            this.type = "text";
            this.label = label;
            this.order = order;
            this.value = value;
        }
    }
}

