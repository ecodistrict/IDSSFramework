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
    /// The base-class to all result massages that is attached to a specific kpi value.
    /// </summary>
    /// <remarks>
    /// Sub-class to <see cref="Result"/>
    /// </remarks>
    /// <seealso cref="Result"/><seealso cref="Outputs"/><seealso cref="Kpi"/>
    [DataContract]
    public class ModuleResult : Result
    {
        /// <summary>
        /// The kpi id that this result refers to.
        /// </summary>
        [DataMember]
        protected string kpiId;

        /// <summary>
        /// The unique identifier of the module.
        /// </summary>
        /// <remarks>
        /// Needed by that the dashboard in order to distinguish between different modules.
        /// </remarks>
        [DataMember]
        protected string moduleId;

        /// <summary>
        /// The variant id aquired from the dashboard in the <see cref="StartModuleRequest"/> message.
        /// </summary>
        [DataMember]
        protected string variantId;

        /// <summary>
        /// The outputs that will be send and visualised in the dashboard.
        /// </summary>
        [DataMember]
        protected Outputs outputs;

        /// <summary>
        /// Defines the ModuleResult constructor.
        /// Can be seralized to a json-string that can be interpeted by the dashboard.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="variantId">The variant id aquired from the dashboard in the <see cref="StartModuleRequest"/> message.</param>
        /// <param name="kpiId">The kpi id that this result refers to.</param>
        /// <param name="outputs">The outputs that will be send and visualised in the dashboard.</param>
        public ModuleResult(string moduleId, string variantId, string kpiId, Outputs outputs)
        {
            this.method = "ModuleResult";
            this.type = "result";
            this.moduleId = moduleId;
            this.kpiId = kpiId;
            this.variantId = variantId;
            this.outputs = outputs;

        }
    }
}
