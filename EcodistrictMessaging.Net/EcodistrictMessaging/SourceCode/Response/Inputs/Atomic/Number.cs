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
    /// </summary>
    /// <remarks>
    /// May be used to define the input specification <see cref="InputSpecification"/>.
    /// 
    /// In that case it will may be visualized as a numericUpDown.
    /// </remarks>
    [DataContract]
    public class Number : Atomic
    {
        /// <summary>
        /// The unit of the number, e.g. "ICQU (International Cheese Quality Units)".
        /// </summary>
        /// <remarks>
        /// May be omitted.
        /// </remarks>
        [DataMember]
        protected object unit;

        /// <summary>
        /// The minimum allowed value of the number.
        /// </summary>
        /// <remarks>
        /// May be omitted.
        /// </remarks>
        [DataMember]
        protected object min;

        /// <summary>
        /// The maximum allowed value of the number.
        /// </summary>
        /// <remarks>
        /// May be omitted.
        /// </remarks>
        [DataMember]
        protected object max;

        internal Number() { }

        /// <summary>
        /// Number constructor.
        /// </summary>
        /// <param name="label">Mandatory label of the visualized component, e.g. "Energy consumption".</param>
        /// <param name="order">Order in which this component should be rendered in the dashboard (ascending order).
        /// Left out or null value will be interpeted as 0 in the dashboard.</param>
        /// <param name="value">Initial value, between min and max (if these are supplied).</param>
        /// <param name="unit">String indicating the unit of the number, e.g. kWh/year.</param>
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

        /// <summary>
        /// Indicator whether <see cref="Serialize"/> should serialize the property <see cref="unit"/>.
        /// </summary>
        /// <remarks>
        /// Is <see cref="Boolean">false</see> if <see cref="unit"/> is omitted in the constructor.
        /// </remarks>
        /// <returns> 
        /// <see cref="Boolean">true</see> if the class property <see cref="unit"/>  should be serialized with the <see cref="Input"/> object.
        /// </returns>
        public bool ShouldSerializeunit()
        {
            return unit != null;
        }

        /// <summary>
        /// Indicator whether <see cref="Serialize"/> should serialize the property <see cref="min"/>.
        /// </summary>
        /// <remarks>
        /// Is <see cref="Boolean">false</see> if <see cref="min"/> is omitted in the constructor.
        /// </remarks>
        /// <returns> 
        /// <see cref="Boolean">true</see> if the class property <see cref="min"/>  should be serialized with the <see cref="Input"/> object.
        /// </returns>
        public bool ShouldSerializemin()
        {
            return min != null;
        }

        /// <summary>
        /// Indicator whether <see cref="Serialize"/> should serialize the property <see cref="max"/>.
        /// </summary>
        /// <remarks>
        /// Is <see cref="Boolean">false</see> if <see cref="max"/> is omitted in the constructor.
        /// </remarks>
        /// <returns> 
        /// <see cref="Boolean">true</see> if the class property <see cref="max"/>  should be serialized with the <see cref="Input"/> object.
        /// </returns>
        public bool ShouldSerializemax()
        {
            return max != null;
        }

    }
}
