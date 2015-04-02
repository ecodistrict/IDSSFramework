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
    /// into a json-message that can be sent to the dashboard as a response to the <see cref=" GetModulesRequest"/>.
    /// </summary>
    /// <example>
    /// An example of a response created when the application recieved a <see cref="GetModulesRequest"/> from the 
    /// dashboard. The response message should be sent trough a IMB-hub as a <see cref="T:byte[]"/> originated from 
    /// a json-string. <br/>
    /// <br/>
    /// However, in this example we demonstrates the usage of the .Net message-type <see cref="GetModulesResponse"/>
    /// and how it can be seralized into a valid json-string that can be interpeted by the dashboard..
    /// <code>
    /// //List of kpis this module can calculate.;
    /// List&lt;string&gt; kpiList = new List&lt;string&gt; { "cheese-taste-kpi", "cheese-price-kpi" };
    /// 
    /// //Create the IMessage response.
    /// GetModulesResponse mResponse = 
    ///    new GetModulesResponse(
    ///        name: "Cheese Module", 
    ///        moduleId: "foo-bar_cheese-Module-v1-0",
    ///        description: "A Module to assess cheese quality.", 
    ///        kpiList: kpiList);
    ///
    /// //Seralize the IMessage into a json-string that can be interpeted by the dashboard
    /// //In this case indented in order for it to be easier to read (won't efect the dashboard). 
    /// string message = Serialize.ToJsonString(mResponse,true); 
    ///
    /// //Write the message to the console
    /// Console.WriteLine(message);
    ///
    /// //Output:
    /// //{
    /// //  "name": "Cheese Module",
    /// //  "description": "A Module to assess cheese quality.",
    /// //  "kpiList": [
    /// //    "cheese-taste-kpi",
    /// //    "cheese-price-kpi"
    /// //  ],  
    /// //  "moduleId": "foo-bar_cheese-Module-v1-0",  
    /// //  "method": "getModules",
    /// //  "type": "response"
    /// //}
    /// </code>
    /// </example>
    /// <seealso cref="IMessage"/>
    /// <seealso cref="Deserialize"/>
    /// <seealso cref="SelectModuleRequest"/>
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
