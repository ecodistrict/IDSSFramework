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
    /// Derived from the class <see cref="Response"/> and is a .Net container that can be seralized 
    /// to a json-message that can be sent to the dashboard as a response to the "getModules" request
    /// <see cref=" GetModulesRequest"/>.
    /// </summary>
    [DataContract]
    public class GetModulesResponse : Response
    {
        /// <summary>
        /// Name of the Module, will be visualized in the dashboard.
        /// </summary>
        [DataMember]
        protected string name;

        /// <summary>
        /// A description of the Module that will be visualized in the dashboard.
        /// </summary>
        [DataMember]
        protected string description;

        /// <summary>
        /// A list of kpis that the module can supply.
        /// </summary>
        [DataMember]
        protected List<string> kpiList = new List<string>();

        internal GetModulesResponse() { }

        /// <summary>
        /// Defines the response to the "getModules" broadcast request sent by the dashboard.
        /// This class contains the Module information needed, and can be seralized to a 
        /// json-string that can be interpeted by the dashboard.
        /// </summary>
        /// <param name="name">Name of the Module, will be visualized in the dashboard.</param>
        /// <param name="moduleId">Unique identifier of the Module (Web-friendly string).</param>
        /// <param name="description">A description of the Module that will be visualized in the dashboard.</param>
        /// <param name="kpiList">A list of kpis that the Module can calculate.</param>
        public GetModulesResponse(string name, string moduleId, 
            string description, List<string> kpiList)
        {
            this.method = "getModules";
            this.type = "response";
            this.name = name;
            this.description = description;
            this.moduleId = moduleId;
            this.kpiList = kpiList;

        }
            

    }
}
