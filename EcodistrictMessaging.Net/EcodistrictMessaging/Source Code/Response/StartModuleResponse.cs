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
    /// to a json-message that can be sent to the dashboard as a response to the "startModule" request
    /// <see cref=" StartModuleRequest"/>.
    /// </summary>
    /// <remarks>
    /// For a "startModule" request the dashboard wants 2 responses.
    /// 1. When the Module starts computing the output data: Response with status="processing"
    /// 2. When the Module have succesfully finished computing the output data: Response with status="success"
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
    /// <seealso cref="ModuleResult"/>
    [DataContract]
    public class StartModuleResponse : Response
    {
        /// <summary>
        /// The kpi id that this response refers to.
        /// </summary>
        [DataMember]
        protected string kpiId;

        /// <summary>
        /// The variant id aquired from the dashboard in the <see cref="StartModuleRequest"/> message.
        /// </summary>
        [DataMember]
        protected string variantId;

        /// <summary>
        /// The progress of the calculation of the kpi.
        /// </summary>
        [DataMember]
        protected string status;

        internal StartModuleResponse() { }

        /// <summary>
        /// Defines the response to the "startModule" request sent by the dashboard.
        /// Can be seralized to a json-string that can be interpeted by the dashboard.
        /// </summary>
        /// <param name="moduleId">Unique identifer of the module using the messaging protokoll.</param>
        /// <param name="variantId">Used by dashboard for tracking.</param>
        /// <param name="kpiId">The kpi that the dashboard previously selected.</param>
        /// <param name="status">Status message</param>
        public StartModuleResponse(string moduleId, string variantId, string kpiId, ModuleStatus status)
        {
            this.method = "startModule";
            this.type = "response";
            this.moduleId = moduleId;
            this.variantId = variantId;
            this.kpiId = kpiId;

            if (status == ModuleStatus.Processing)
                this.status = "processing";
            else if (status == ModuleStatus.Success)
                this.status = "success";
        }
    }

    /// <summary>
    /// Indicator used by <see cref="StartModuleResponse"/> in order signal the 
    /// state of the module.
    /// </summary>
    public enum ModuleStatus
    {
        /// <summary>
        /// The module is calculating. 
        /// </summary>
        Processing,

        /// <summary>
        /// The module has finished succesfully, the dashboard may expect a result soon.
        /// </summary>
        Success
    }
}
