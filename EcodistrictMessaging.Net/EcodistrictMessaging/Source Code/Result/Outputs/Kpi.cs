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
    /// A sub-class of <see cref="Output"/>. 
    /// 
    /// Consist of a single value with a description and unit.
    /// </summary>
    [DataContract]
    public class Kpi : Output
    {
        /// <summary>
        /// The value of the kpi.
        /// </summary>
        [DataMember]
        protected object value;

        /// <summary>
        /// Information about the kpi, e.g. "Cheese tastiness".
        /// </summary>
        [DataMember]
        protected string info;

        /// <summary>
        /// The unit of the kpi value, e.g. "ICQU (International Cheese Quality Units)".
        /// </summary>
        [DataMember]
        protected string unit;

        internal Kpi() { }
        
        /// <summary>
        /// The constructor for the kpi.
        /// </summary>
        /// <param name="value">The value of the kpi.</param>
        /// <param name="info">Information about the kpi, e.g. "Cheese tastiness".</param>
        /// <param name="unit">The unit of the kpi value, e.e. "ICQU (International Cheese Quality Units)".</param>
        public Kpi(object value, string info, string unit)
        {
            this.type = "kpi";
            this.value = value;
            this.info = info;
            this.unit = unit;

        }

    }
}
