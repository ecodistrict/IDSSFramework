using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Ecodistrict.Messaging.Responses
{
    /// <summary>
    /// Derived from the class <see cref="Response"/> and is a .Net container that can be seralized 
    /// to a json-message that can be sent to the dashboard as a response to the "selectModule" request
    /// <see cref=" SelectModuleRequest"/>.
    /// </summary>
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
    ///                    "\"method\": \"selectModule\"," +
    ///                    "\"moduleId\": \"foo-bar_cheese-Module-v1-0\"," +
    ///                    "\"variantId\": \"503f191e8fcc19729de860ea\"," +
    ///                    "\"kpiId\": \"cheese-taste-kpi\"" +
    ///                  "}";
    /// //Message reconstructed into a .Net object.
    /// SelectModuleRequest recievedMessage = (SelectModuleRequest)Deserialize.JsonString(message);
    ///
    /// //Is this message meant for me?
    /// if (recievedMessage.moduleId == moduleId)
    /// {
    ///    //For the selected kpi, create a input specification describing what data 
    ///    //the module need in order to calculate the selected kpi.
    ///    InputSpecification iSpec = new InputSpecification();
    ///    if (recievedMessage.kpiId == "cheese-taste-kpi")
    ///    {
    ///        //In this case the module needs 2 things.
    ///
    ///        //A user specified age
    ///        Number numberAge = new Number(
    ///            label: "Age",
    ///            min: 0,
    ///            unit: "years");
    ///
    ///        Options opt = new Options();
    ///        opt.Add(new Option(value: "alp-cheese", label: "Alpk\u00e4se")); //Note the web-friendly string
    ///        opt.Add(new Option(value: "edam-cheese", label: "Edammer"));
    ///        opt.Add(new Option(value: "brie-cheese", label: "Brie"));
    ///
    ///        //And one of the above options of cheese-types. 
    ///        //(The preselected value, "brie-cheese", is optional)
    ///        Select selectCheseType = new Select(
    ///            label: "Cheese type",
    ///            options: opt,
    ///            value: "brie-cheese");
    ///
    ///        //Add these components to the input specification.
    ///        //(Note the choosed keys, its the keys that will be attached to the
    ///        //data when the dashboard returns with the user specified data in
    ///        //a StartModuleRequest.)
    ///        iSpec.Add(
    ///            key: "age",
    ///            value: numberAge);
    ///
    ///        iSpec.Add(
    ///            key: "cheese-type",
    ///            value: selectCheseType);
    ///    }
    ///    else
    ///    {
    ///        //...
    ///    }
    ///
    ///    //Create the IMessage response.
    ///    SelectModuleResponse mResponse = new SelectModuleResponse(
    ///        moduleId: recievedMessage.moduleId,
    ///        variantId: recievedMessage.variantId,
    ///        kpiId: recievedMessage.kpiId,
    ///        inputSpecification: iSpec);
    ///
    ///    //json-string that can be interpeted by the dashboard
    ///    //In this case indented in order to be easier to read (won't efect the dashboard). 
    ///    string messageResponse = Serialize.ToJsonString(mResponse, true);
    ///
    ///    //Write the message to the console
    ///    Console.WriteLine(messageResponse);
    ///
    ///    //Output:
    ///    //{
    ///    //  "kpiId": "cheese-taste-kpi",
    ///    //  "variantId": "503f191e8fcc19729de860ea",
    ///    //  "inputSpecification": {
    ///    //    "age": {
    ///    //      "unit": "years",
    ///    //      "min": 0,
    ///    //      "label": "Age",
    ///    //      "type": "number"
    ///    //    },
    ///    //    "cheese-type": {
    ///    //      "options": [
    ///    //        {
    ///    //          "value": "alp-cheese",
    ///    //          "label": "Alpk\u00e4se"
    ///    //        },
    ///    //        {
    ///    //          "value": "edam-cheese",
    ///    //          "label": "Edammer"
    ///    //        },
    ///    //        {
    ///    //          "value": "brie-cheese",
    ///    //          "label": "Brie"
    ///    //        }
    ///    //      ],
    ///    //      "value": "brie-cheese",
    ///    //      "label": "Cheese type",
    ///    //      "type": "select"
    ///    //    }	
    ///    //  },  
    ///    //  "moduleId": "foo-bar_cheese-Module-v1-0",
    ///    //  "method": "selectModule",
    ///    //  "type": "response"
    ///    //}
    ///
    ///}
    /// </code>
    /// </example>
    /// <seealso cref="IMessage"/>
    /// <seealso cref="SelectModuleRequest"/>
    /// <seealso cref="Serialize"/>
    [DataContract]
    public class SelectModuleResponse : Response
    {
        /// <summary>
        /// The kpi id that this response refers to.
        /// </summary>
        [DataMember]
        protected string kpiId;

        /// <summary>
        /// The variant id aquired from the dashboard in the <see cref="SelectModuleRequest"/> message.
        /// </summary>
        [DataMember]
        protected string variantId;

        /// <summary>
        /// The input specification for the selected kpi.
        /// </summary>
        /// <remarks>
        /// The input specification describes what the module need in order to
        /// calculate a given kpi.
        /// </remarks>
        [DataMember]
        protected InputSpecification inputSpecification;

        internal SelectModuleResponse() { }

        /// <summary>
        /// Defines the response to the "selectModule" request sent by the dashboard.
        /// Can be seralized to a json-string that can be interpeted by the dashboard.
        /// </summary>
        /// <param name="moduleId">Unique identifer of the module using the messaging protokoll.</param>
        /// <param name="variantId">Used by dashboard for tracking.</param>
        /// <param name="kpiId">The kpi that the dashboard previously selected.</param>
        /// <param name="inputSpecification">The specification to the dashboard on what data the Module need.</param>
        public SelectModuleResponse(string moduleId, string variantId, string kpiId, InputSpecification inputSpecification)
        {
            this.method = "selectModule";
            this.type = "response";
            this.moduleId = moduleId;
            this.kpiId = kpiId;
            this.variantId = variantId;
            this.inputSpecification = inputSpecification;
        }
    }
}
