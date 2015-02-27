using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using System.Reflection;

using System.CodeDom;

using Ecodistrict.Messaging;

namespace TestConsole
{

    [DataContract]
    public class Test
    {
        [DataMember]
        //dynamic obj = new System.Dynamic.ExpandoObject();
        dynamic obj = new System.Dynamic.ExpandoObject();// as IDictionary<string, Object>;

        public Test()
        {
            var dict = (IDictionary<string, object>)obj;
            dict["bar"] = 123;

            //((IDictionary<string, Object>)obj).Add("NewProp", "something");
            //obj.apa = 1;
            
            //Type myType = obj.GetType();
            //PropertyInfo pinfo = myType.GetProperty("Caption");
            //pinfo.SetValue(obj, "something", null);

            //obj.GetType().GetProperty("bar").SetValue(this, "something", null);
            //this.apa = "string";
            
            //this = new System.Dynamic.ExpandoObject(); 
            //obj.apa = "string";

            CodeVariableDeclarationStatement variableDeclaration = new CodeVariableDeclarationStatement(
                // Type of the variable to declare. 
        typeof(string),
                // Name of the variable to declare. 
        "TestString",
                // Optional initExpression parameter initializes the variable. 
        new CodePrimitiveExpression("Testing"));
        }
    }

    class Program
    {

        void JsonSerializeTest()
        {
            var settings = new DataContractJsonSerializerSettings();
            settings.EmitTypeInformation = EmitTypeInformation.Never;
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Test), settings);

            Test obj = new Test();

            ser.WriteObject(stream1, obj);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            Console.WriteLine(sr.ReadToEnd());

            dynamic foo = new System.Dynamic.ExpandoObject();
            foo.Bar = "something";
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(foo);
            Console.WriteLine(json);
        }

        void JsonSeralizeISpec()
        {
            var settings = new DataContractJsonSerializerSettings();
            settings.EmitTypeInformation = EmitTypeInformation.Never;
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(InputSpecification), settings);
            InputSpecification inputSpec = new InputSpecification();

            inputSpec.Add(new Number(label: "A number", id: "1"));
            List listRoot = new List(label: "A list", id: "2");
            List aList = new List(label: "A list", id: "3");
            aList.Add(new Number(label: "A number", id: "4"));
            aList.Add(new Number(label: "A number", id: "5"));
            listRoot.Add(aList);
            inputSpec.Add(listRoot);

            ser.WriteObject(stream1, inputSpec);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            Console.WriteLine(sr.ReadToEnd());
        }

        static void Main(string[] args)
        {
            try
            {
                Program prg = new Program();
                prg.JsonSeralizeISpec();
                prg.JsonSerializeTest();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }
    }
}
