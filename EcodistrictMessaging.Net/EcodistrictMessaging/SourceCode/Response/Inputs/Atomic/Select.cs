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
    /// May be used to define the <see cref="InputSpecification"/>.
    /// 
    /// In that case it will be displayed as a drop down list of values, 
    /// from which the user may select one.
    /// (Renders as a drop-down list (HTML {select} tag))
    /// </remarks>
    [DataContract]
    public class Select : Atomic
    {
        /// <summary>
        /// Defines the options visualized in selection box in the dashboard.
        /// </summary>
        [DataMember]
        protected Options options;

        internal Select() { }

        /// <summary>
        /// Select constructor.
        /// </summary>
        /// <param name="label">Mandatory label of the visualized component, e.g. "Energy consumption".</param>
        /// <param name="options">Mandatory list of objects (defined by <see cref="Options"/>) from which the user may choose from.</param>
        /// <param name="order">Order in which this component should be rendered in the dashboard (ascending order).
        /// Left out or null value will be interpreted as 0 in the dashboard. </param>
        /// <param name="value">If not null, must be an option equal to one of the options.</param>
        public Select(string label, Options options, int? order = null, Option value = null)
        {
            this.type = "select";
            this.label = label;
            this.order = order;
            if (value != null)
            {
                if (options.Contains(value))
                    this.value = value.value;
                else
                    throw new Exception(String.Format("In Select: Selected default option, {0}, is not among the supplied options."));
            }
            this.options = options;
        }
    }
}
