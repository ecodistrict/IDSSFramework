using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using Ecodistrict.Messaging;

using Newtonsoft.Json;

namespace TestConsole
{   
    class Program
    {

        static void InputSpecificationTest()
        {
            try
            {
                InputSpecification inputSpec = new InputSpecification();
                inputSpec.Add("name", new Text(label: "Parent name"));
                inputSpec.Add("age", new Number(label: "Parent age"));
                List aList = new List("Children");
                aList.Add("name", new Text(label: "Child name"));
                aList.Add("age", new Number(label: "Child age"));
                inputSpec.Add("child", aList);

                Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(inputSpec, typeof(InputSpecification), settings));
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void IMessageTest()
        {
            try
            {
                string smessage = "{\"method\":\"getModules\",\"type\":\"request\"}";
                //IMessage message = (IMessage)Newtonsoft.Json.JsonConvert.DeserializeObject(smessage, typeof(IMessage));

                //object message = Types.ParseJsonMessage(smessage);

                IMessage message = Deserialize.JsonString(smessage);

                Type type = message.GetType();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static public void Test()
        {
            try
            {
                //Input Spec
                InputSpecification iSpec = new InputSpecification();
                iSpec.Add("age", new Number(label: "Age", min: 0, unit: "years"));
                List aList = new List("Cheese Types");
                aList.Add("alp", new Text("Alp cheese"));
                aList.Add("brie", new Text("Brie cheese"));
                iSpec.Add("cheese-types", aList);
                SelectModuleResponse mResponse = new SelectModuleResponse(moduleId: "foo-bar_cheese-Module-v1-0",
                    variantId: "503f191e8fcc19729de860ea", kpiId: "cheese-taste-kpi", inputSpecification: iSpec);
                string json = Serialize.ToJsonString(mResponse);

                //Request from dashboard
                string jsonmessage = File.ReadAllText(@"../../../EcodistrictMessagingTests/TestData/Json/ModuleRequest/StartModuleRequest2.txt");
                object obj = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonmessage);
                jsonmessage = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                Type expected = typeof(StartModuleRequest);
                IMessage message = Deserialize.JsonString(jsonmessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        #region Examples
        #region Requests
        static void GetModulesRequestExemple()
        {
            //json-string from dashboard
            string message = "{\"method\": \"getModules\",\"type\": \"request\"}";
            //Message reconstructed into a .Net object.
            IMessage recievedMessage = Deserialize.JsonString(message);
            //Write object type to console
            Console.WriteLine(recievedMessage.GetType().ToString());
            //Output: Ecodistrict.Messaging.GetModulesRequest

            Console.WriteLine("");
        }

        static void SelectModuleRequestExemple()
        {
            //json-string from dashboard
            string message = "{"+
                               "\"type\": \"request\"," +
                               "\"method\": \"selectModule\"," +
                               "\"moduleId\": \"foo-bar_cheese-Module-v1-0\"," +
                               "\"variantId\": \"503f191e8fcc19729de860ea\"," +
                               "\"kpiId\": \"cheese-taste-kpi\"" +
                             "}";
            //Message reconstructed into a .Net object.
            IMessage recievedMessage = Deserialize.JsonString(message);
            //Write object type to console
            Console.WriteLine(recievedMessage.GetType().ToString());
            //Output: Ecodistrict.Messaging.SelectModuleRequest

            Console.WriteLine("");
        }

        static void StartModuleRequestExemple()  //TODO update examples for StartModuleRequest. Does not conform with dashboard.
        {
            //json-string from dashboard
            string message = "{" +
                               "\"type\": \"request\"," +
                               "\"method\": \"startModule\"," +
                               "\"moduleId\": \"foo-bar_cheese-Module-v1-0\"," +
                               "\"variantId\": \"503f191e8fcc19729de860ea\"," +
                               "\"kpiId\": \"cheese-taste-kpi\"," +
                               "\"inputData\": {" +
                                                "\"cheese-type\": \"alp-kase\"," +
                                                "\"age\": 2.718" +
                                              "}" +
                            "}";
            //Message reconstructed into a .Net object.
            IMessage recievedMessage = Deserialize.JsonString(message);
            //Write object type to console
            Console.WriteLine(recievedMessage.GetType().ToString());
            //Output: Ecodistrict.Messaging.StartModuleRequest

            Console.WriteLine("");
        }
        #endregion

        #region Responses
        static void GetModulesResponseExemple()
        {
            //List of kpis this module can calculate.
            List<string> kpiList = new List<string> { "cheese-taste-kpi", "cheese-price-kpi" };

            //Create the IMessage response.
            GetModulesResponse mResponse = 
                new GetModulesResponse(
                    name: "Cheese Module", 
                    moduleId: "foo-bar_cheese-Module-v1-0",
                    description: "A Module to assess cheese quality.", 
                    kpiList: kpiList);

            //json-string that can be interpeted by the dashboard
            //In this case indented in order to be easier to read (won't efect the dashboard). 
            string message = Serialize.ToJsonString(mResponse,true); 

            //Write the message to the console
            Console.WriteLine(message);

            //Output:
            //{
            //  "name": "Cheese Module",
            //  "description": "A Module to assess cheese quality.",
            //  "kpiList": [
            //    "cheese-taste-kpi",
            //    "cheese-price-kpi"
            //  ],  
            //  "moduleId": "foo-bar_cheese-Module-v1-0",  
            //  "method": "getModules",
            //  "type": "response"
            //}

            Console.WriteLine("");
        }

        static void SelectModuleResponseExemple()
        {
            //This module's id
            string moduleId = "foo-bar_cheese-Module-v1-0";

            //The dashboard message recieved (as a json-string)
            string message = "{" +
                               "\"type\": \"request\"," +
                               "\"method\": \"selectModule\"," +
                               "\"moduleId\": \"foo-bar_cheese-Module-v1-0\"," +
                               "\"variantId\": \"503f191e8fcc19729de860ea\"," +
                               "\"kpiId\": \"cheese-taste-kpi\"" +
                             "}";
            //Message reconstructed into a .Net object.
            SelectModuleRequest recievedMessage = (SelectModuleRequest)Deserialize.JsonString(message);

            //Is this message meant for me?
            if (recievedMessage.moduleId == moduleId)
            {
                //For the selected kpi, create a input specification describing what data 
                //the module need in order to calculate the selected kpi.
                InputSpecification iSpec = new InputSpecification();
                if (recievedMessage.kpiId == "cheese-taste-kpi")
                {
                    //In this case the module needs 2 things.

                    //A user specified age
                    Number numberAge = new Number(
                        label: "Age",
                        min: 0,
                        unit: "years");

                    Options opt = new Options();
                    opt.Add(new Option(value: "alp-cheese", label: "Alpk\u00e4se")); //Note the web-friendly string
                    opt.Add(new Option(value: "edam-cheese", label: "Edammer"));
                    opt.Add(new Option(value: "brie-cheese", label: "Brie"));

                    //And one of the above options of cheese-types. 
                    //(The preselected value, "brie-cheese", is optional)
                    Select selectCheseType = new Select(
                        label: "Cheese type",
                        options: opt,
                        value: "brie-cheese");

                    //Add these components to the input specification.
                    //(Note the choosed keys, its the keys that will be attached to the
                    //data when the dashboard returns with the user specified data in
                    //a StartModuleRequest.)
                    iSpec.Add(
                        key: "age",
                        value: numberAge);

                    iSpec.Add(
                        key: "cheese-type",
                        value: selectCheseType);
                }
                else
                {
                    //...
                }

                //Create the IMessage response.
                SelectModuleResponse mResponse = new SelectModuleResponse(
                    moduleId: recievedMessage.moduleId,
                    variantId: recievedMessage.variantId,
                    kpiId: recievedMessage.kpiId,
                    inputSpecification: iSpec);

                //json-string that can be interpeted by the dashboard
                //In this case indented in order to be easier to read (won't efect the dashboard). 
                string messageResponse = Serialize.ToJsonString(mResponse, true);

                //Write the message to the console
                Console.WriteLine(messageResponse);

                //Output:
                //{
                //  "kpiId": "cheese-taste-kpi",
                //  "variantId": "503f191e8fcc19729de860ea",
                //  "inputSpecification": {
                //    "age": {
                //      "unit": "years",
                //      "min": 0,
                //      "label": "Age",
                //      "type": "number"
                //    },
                //    "cheese-type": {
                //      "options": [
                //        {
                //          "value": "alp-cheese",
                //          "label": "Alpk\u00e4se"
                //        },
                //        {
                //          "value": "edam-cheese",
                //          "label": "Edammer"
                //        },
                //        {
                //          "value": "brie-cheese",
                //          "label": "Brie"
                //        }
                //      ],
                //      "value": "brie-cheese",
                //      "label": "Cheese type",
                //      "type": "select"
                //    }	
                //  },  
                //  "moduleId": "foo-bar_cheese-Module-v1-0",
                //  "method": "selectModule",
                //  "type": "response"
                //}

            }

            Console.WriteLine("");

        }

        static void StartModuleResponseExemple()
        {
            //This module's id
            string moduleId = "foo-bar_cheese-Module-v1-0";

            //The dashboard message recieved (as a json-string)
            string message = "{" +
                               "\"type\": \"request\"," +
                               "\"method\": \"startModule\"," +
                               "\"moduleId\": \"foo-bar_cheese-Module-v1-0\"," +
                               "\"variantId\": \"503f191e8fcc19729de860ea\"," +
                               "\"kpiId\": \"cheese-taste-kpi\"," +
                               "\"inputData\": {" +
                                                "\"cheese-type\": \"alp-kase\"," +
                                                "\"age\": 2.718" +
                                              "}" +
                            "}";
            //Message reconstructed into a .Net object.
            StartModuleRequest recievedMessage = (StartModuleRequest)Deserialize.JsonString(message);

            //Is this message meant for me?
            if (recievedMessage.moduleId == moduleId)
            {
                //For the selected kpi, create a input specification describing what data 
                //the module need in order to calculate the selected kpi.
                Outputs outputs = new Outputs();
                if (recievedMessage.kpiId == "cheese-taste-kpi")
                {
                    //Inform the dashboard that you have started calculating
                    StartModuleResponse mResponse = new StartModuleResponse(
                        moduleId: recievedMessage.moduleId,
                        variantId: recievedMessage.variantId,
                        kpiId: recievedMessage.kpiId,
                        status: ModuleStatus.Processing);
                    //Send the response message...

                    //Do your calculations...

                    //Inform the dashboard that you have finnished the calculations
                    mResponse = new StartModuleResponse(
                        moduleId: recievedMessage.moduleId,
                        variantId: recievedMessage.variantId,
                        kpiId: recievedMessage.kpiId,
                        status: ModuleStatus.Success);
                    //Send the response message...


                    //Add the result in outputs
                    //E.g.
                    Output otp = new Kpi(
                        value: 99, 
                        info:"Cheese tastiness", 
                        unit:"ICQU (International Cheese Quality Units)");
                    outputs.Add(otp);
                }
                else
                {
                    //...
                }

                //Inform the dashboard of your results
                ModuleResult mResult = new ModuleResult(
                        moduleId: recievedMessage.moduleId,
                        variantId: recievedMessage.variantId,
                        kpiId: recievedMessage.kpiId,
                        outputs: outputs);
                //Send the result message...


                string resMessage = Serialize.ToJsonString(mResult);
                Console.WriteLine(resMessage);


            }
        }

        #endregion

        static void InputSpecificationExample()
        {
            //Create a input specification demanding 2 values from the user of the dashboard.
            InputSpecification iSpec = new InputSpecification();

            //A user specified age
            Number numberAge = new Number(
                label: "Age",
                min: 0,
                unit: "years");

            Options opt = new Options();
            opt.Add(new Option(value: "alp-cheese", label: "Alpk\u00e4se")); //Note the web-friendly string
            opt.Add(new Option(value: "edam-cheese", label: "Edammer"));
            opt.Add(new Option(value: "brie-cheese", label: "Brie"));

            //And one of the above options of cheese-types. 
            //(The preselected value, "brie-cheese", is optional)
            Select selectCheseType = new Select(
                label: "Cheese type",
                options: opt,
                value: "brie-cheese");

            //Add these components to the input specification.
            //(Note the choosed keys, its the keys that will be attached to the
            //data when the dashboard returns with the user specified data in
            //a StartModuleRequest.)
            iSpec.Add(
                key: "age",
                value: numberAge);

            iSpec.Add(
                key: "cheese-type",
                value: selectCheseType);
        }

        #endregion

        
        static void StartModuleRequestComplex()
        {
            //json-string from dashboard
            string message = File.ReadAllText(@"../../../EcodistrictMessagingTests/TestData/Json/ModuleRequest/StartModuleRequestComplex.txt");
            //Message reconstructed into a .Net object.
            IMessage recievedMessage = Deserialize.JsonString(message);
            //Write object type to console
            Console.WriteLine(recievedMessage.GetType().ToString());
            //Output: Ecodistrict.Messaging.StartModuleRequest

            Console.WriteLine("");
        }

                        


        static void StartModuleResponseReconstructionTest()
        {
            // arrange
            InputSpecification inputSpec = new InputSpecification();
            List aList = new List(label: "aList");
            aList.Add(key: "o1", item: new Number(label: "o1 label", value: 1));
            aList.Add(key: "o2", item: new Number(label: "o2 label", value: 2));
            aList.Add(key: "o3", item: new Number(label: "o3 label", value: 3));
            inputSpec.Add("list", aList);
            SelectModuleResponse mResponse = new SelectModuleResponse("", "", "", inputSpec);
            string expected = Serialize.ToJsonString(mResponse);

            // act
            SelectModuleResponse mResponseR = (SelectModuleResponse)Deserialize.JsonString(expected);
            string actual = Serialize.ToJsonString(mResponseR);
        }

        static void Main(string[] args)
        {
            try
            {
                GetModulesRequestExemple();
                SelectModuleRequestExemple();
                StartModuleRequestExemple();
                GetModulesResponseExemple();
                SelectModuleResponseExemple();
                StartModuleResponseExemple();

                //StartModuleResponseReconstructionTest();

                // arrange
                string jsonmessage = File.ReadAllText(@"../../../EcodistrictMessagingTests/TestData/Json/ModuleRequest/StartModuleRequest3.txt");
               
                // act
                IMessage message = Deserialize.JsonString(jsonmessage);
                Type actual = message.GetType();


                StartModuleRequestComplex();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }        

        }
    }
}
