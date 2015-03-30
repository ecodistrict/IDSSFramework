using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using Ecodistrict.Messaging;

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

                IMessage message = Deserialize.JsonMessage(smessage);

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
                IMessage message = Deserialize.JsonMessage(jsonmessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void Main(string[] args)
        {
            //InputSpecificationTest();
            //IMessageTest();
            //Test();
                       
        }
    }
}
