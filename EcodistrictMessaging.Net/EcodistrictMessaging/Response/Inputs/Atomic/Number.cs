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
    /// Input class derived from the class <see cref="Atomic"/>. 
    /// </summary>
    /// <remarks>
    /// May be used to define the input specification <see cref="InputSpecification"/>.
    /// 
    /// In that case it will may be visualized as a numericUpDown.
    /// </remarks>
    [DataContract]
    public class Number : Atomic
    {
        [DataMember]
        object unit;
        [DataMember]
        object min;
        [DataMember]
        object max;

        /// <summary>
        /// Number constructor.
        /// </summary>
        /// <param name="label">Mandatory label of the component, e.g. "Energy consumption".</param>
        /// <param name="order">Order in which this component should be rendered in the dashboard (ascending order).</param>
        /// <param name="value">Initial value, between min and max (if these are supplied).</param>
        /// <param name="unit">String indication the unit of the number, e.g. kWh/year.</param>
        /// <param name="min">Inclusive min. Default: no limit.</param>
        /// <param name="max">Inclusve max. Default: no limit.</param>
        public Number(string label, int? order = null, object value = null,
            object unit = null, object min = null, object max = null)
        {
            this.type = "number";
            this.label = label;
            this.order = order;
            this.value = value;
            this.unit = unit;
            this.min = min;
            this.max = max;
        }

        public bool ShouldSerializeunit()
        {
            return unit != null;
        }

        public bool ShouldSerializemin()
        {
            return min != null;
        }

        public bool ShouldSerializemax()
        {
            return max != null;
        }

    }
}
