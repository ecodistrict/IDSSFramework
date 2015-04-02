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
    ///  <example>
    /// An example of a response created when the application recieved a <see cref="SelectModuleRequest"/> from the 
    /// dashboard. The response message should be sent trough a IMB-hub as a <see cref="T:byte[]"/> originated from 
    /// a json-string. <br/>
    /// <br/>
    /// However, in this example we demonstrates the usage of the .Net message-type <see cref="SelectModuleResponse"/>
    /// and how it can be seralized into a valid json-string that can be interpeted by the dashboard.
    /// <code>
    /// //This module's id
    /// string moduleId = "foo-bar_cheese-Module-v1-0";
    ///
    /// //The dashboard message recieved (as a json-string)
    /// string message = "{" +
    ///                    "\"type\": \"request\"," +
    ///                    "\"method\": \"startModule\"," +
    ///                    "\"moduleId\": \"foo-bar_cheese-Module-v1-0\"," +
    ///                    "\"variantId\": \"503f191e8fcc19729de860ea\"," +
    ///                    "\"kpiId\": \"cheese-taste-kpi\"," +
    ///                    "\"inputData\": {" +
    ///                                     "\"cheese-type\": \"alp-kase\"," +
    ///                                     "\"age\": 2.718" +
    ///                                   "}" +
    ///                 "}";
    /// //Message reconstructed into a .Net object.
    /// StartModuleRequest recievedMessage = (StartModuleRequest)Deserialize.JsonString(message);
    ///
    /// //Is this message meant for me?
    /// if (recievedMessage.moduleId == moduleId)
    /// {
    ///    //For the selected kpi, create a input specification describing what data 
    ///    //the module need in order to calculate the selected kpi.
    ///    Outputs outputs = new Outputs();
    ///    if (recievedMessage.kpiId == "cheese-taste-kpi")
    ///    {
    ///        //Inform the dashboard that you have started calculating
    ///        StartModuleResponse mResponse = new StartModuleResponse(
    ///            moduleId: recievedMessage.moduleId,
    ///            variantId: recievedMessage.variantId,
    ///            kpiId: recievedMessage.kpiId,
    ///            status: ModuleStatus.Processing);
    ///        //Send the response message...
    ///
    ///        //Do your calculations...
    ///
    ///        //Inform the dashboard that you have finnished the calculations
    ///        mResponse = new StartModuleResponse(
    ///            moduleId: recievedMessage.moduleId,
    ///            variantId: recievedMessage.variantId,
    ///            kpiId: recievedMessage.kpiId,
    ///            status: ModuleStatus.Success);
    ///        //Send the response message...
    ///
    ///
    ///        //Add the result in outputs
    ///        //E.g.
    ///        Output otp = new Kpi(
    ///            value: 99, 
    ///            info:"Cheese tastiness", 
    ///            unit:"ICQU (International Cheese Quality Units)");
    ///        outputs.Add(otp);
    ///    }
    ///    else
    ///    {
    ///        //...
    ///    }
    ///
    ///    //Inform the dashboard of your results
    ///    ModuleResult mResult = new ModuleResult(
    ///            moduleId: recievedMessage.moduleId,
    ///            variantId: recievedMessage.variantId,
    ///            kpiId: recievedMessage.kpiId,
    ///            outputs: outputs);
    ///    //Send the result message...
    ///
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="IMessage"/>
    /// <seealso cref="StartModuleRequest"/>
    /// <seealso cref="StartModuleResponse"/>
    /// <seealso cref="Result"/>
    /// <seealso cref="Outputs"/>
    /// <seealso cref="Kpi"/>
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
        protected List<Output> outputs;

        internal ModuleResult() { }

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
