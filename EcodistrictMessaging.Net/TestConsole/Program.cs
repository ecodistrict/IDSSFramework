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
    interface IAnimal
    {
        string Type { get; set; }
    }

    class Cat : IAnimal
    {
        public string CatName { get; set; }
        public string Type { get; set; }
    }

    class Dog : IAnimal
    {
        public string DogName { get; set; }
        public string Type { get; set; }
    }

    class AnimalJson
    {
        public IEnumerable<IAnimal> Items { get; set; }
    }

    class Animal
    {
        public string Type { get; set; }
        public string Name { get; set; }
    }

    class AnimalItemConverter : Newtonsoft.Json.Converters.CustomCreationConverter<IAnimal>
    {
        public override IAnimal Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        public IAnimal Create(Type objectType, Newtonsoft.Json.Linq.JObject jObject)
        {
            var type = (string)jObject.Property("type");

            switch (type)
            {
                case "cat":
                    return new Cat();
                case "dog":
                    return new Dog();
            }

            throw new ApplicationException(String.Format("The animal type {0} is not supported!", type));
        }

        public override object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream 
            Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Load(reader);

            // Create target object based on JObject 
            var target = Create(objectType, jObject);

            // Populate the object properties 
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }

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

        static void Main(string[] args)
        {

            string jsonmessage = File.ReadAllText(@"../../../EcodistrictMessagingTests/TestData/Json/ModuleRequest/StartModuleRequest.txt");
            IMessage objd = Ecodistrict.Messaging.Deserialize.JsonString(jsonmessage);


            //InputSpecificationTest();
            //IMessageTest();
            //Test();

            // arrange
            Outputs outputs = new Outputs();
            outputs.Add(new Kpi(1, "info", "unit"));
            ModuleResult mResult = new ModuleResult("moduleId", "variantId", "KpiId", outputs);
            string str1 = Serialize.ToJsonString(mResult);

            // act
            ModuleResult mResult2 = (ModuleResult)Deserialize.JsonString(str1);
            string str2 = Serialize.ToJsonString(mResult2);


            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
            string strOtps = Newtonsoft.Json.JsonConvert.SerializeObject(outputs, typeof(Outputs), settings);
            Outputs outputs2 = (Outputs)Newtonsoft.Json.JsonConvert.DeserializeObject(strOtps, typeof(Outputs), settings);







            Output output = new Kpi(1, "info", "unit");
            string strOtp = Newtonsoft.Json.JsonConvert.SerializeObject(output, typeof(Output), settings);
            object output2 = JsonConvert.DeserializeObject<Output>(strOtp, new OutputItemConverter());
            //Output output2 = (Kpi)JsonConvert.DeserializeObject(strOtp, typeof(Kpi), settings);
















            string json = "{ items: [{ type: \"cat\", catName: \"tom\" }, { type: \"dog\", dogName: \"fluffy\" }] }";
            object obj = JsonConvert.DeserializeObject<AnimalJson>(json, new AnimalItemConverter());


        }
    }
}
