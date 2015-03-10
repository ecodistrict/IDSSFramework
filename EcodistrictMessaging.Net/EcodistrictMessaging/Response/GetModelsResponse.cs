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
    /// to a json-message that can be sent to the dashboard as a response to the "getModels" request
    /// <see cref=" GetModelsRequest"/>.
    /// </summary>
    [DataContract]
    public class GetModelsResponse : Response
    {
        [DataMember]
        protected string name;
        [DataMember]
        protected string description;
        [DataMember]
        protected List<string> kpiList = new List<string>();

        /// <summary>
        /// Defines the response to the "getModels" broadcast request sent by the dashboard.
        /// This class contains the model information needed, and can be seralized to a 
        /// json-string that can be interpeted by the dashboard.
        /// </summary>
        /// <param name="name">Name of the model, will be visualized in the dashboard.</param>
        /// <param name="moduleId">Unique identifier of the model.</param>
        /// <param name="description">A description of the model that will be visualized in the dashboard.</param>
        /// <param name="kpiList">A list of kpis that the model can calculate.</param>
        public GetModelsResponse(string name, string moduleId, 
            string description, List<string> kpiList)
        {
            this.method = "getModels";
            this.type = "response";
            this.name = name;
            this.description = description;
            this.moduleId = moduleId;
            this.kpiList = kpiList;

        }
            

    }
}
