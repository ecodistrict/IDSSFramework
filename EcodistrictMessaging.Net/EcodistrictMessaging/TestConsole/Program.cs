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

using System.Dynamic;
using System.Linq.Expressions;

using System.Collections;
using System.Collections.Specialized;

using Ecodistrict.Messaging;

namespace TestConsole
{
   
    public class Test
    {
        public dynamic input
        { get; private set; }
        
        public Test()
        {
            input = new OrderedDictionary();
        }

        public void Add(string key, Input item)
        {
            input.Add(key, item);
        }

        public string ToJson()
        {
            string json = "";

            IDictionaryEnumerator myEnumerator = input.GetEnumerator();

            bool done = !myEnumerator.MoveNext();

            while (!done)
            {
                json += string.Format("{0}:{1}", myEnumerator.Key, ((Input)myEnumerator.Value).ToJson());
                done = !myEnumerator.MoveNext();
                if (!done)
                    json += ",";
            }

            json = string.Format("{0}{1}{2}", "{", json, "}");
            return json;
        }
    }

    class Program
    {
        void JsonSerializeTest()
        {            
            Test obj = new Test();

            obj.Add("number1",new Number("aLbl", "aId"));
            obj.Add("number2", new Number("aLbl2", "aId2"));

            Console.WriteLine(obj.ToJson());
            Console.WriteLine("");
        }

        void JsonSeralizeISpec()
        {
            InputSpecification inputSpec = new InputSpecification();
            inputSpec.Add("name", new Text("Name"));
            inputSpec.Add("shoe-size", new Number("Shoe size"));
            Console.WriteLine(inputSpec.ToJson());
            Console.WriteLine("");


            inputSpec = new InputSpecification();
            inputSpec.Add("name", new Text("Parent name"));
            inputSpec.Add("age", new Number("Parent age"));
            List aList = new List("Children");
            aList.Add("name", new Text("Child name"));
            aList.Add("age", new Number("Child age"));
            inputSpec.Add("child", aList);
            Console.WriteLine(inputSpec.ToJson());
            Console.WriteLine("");

            inputSpec = new InputSpecification();
            Options opt = new Options();
            opt.Add(new Option(value: "alp-cheese", label: "Alpk\\u00e4se"));
            opt.Add(new Option(value: "edam-cheese", label: "Edammer"));
            opt.Add(new Option(value: "brie-cheese", label: "Brie"));
            inputSpec.Add(key: "cheese-type", item: new Select(label: "Cheese type", options: opt, value: "brie-cheese"));
            Console.WriteLine(inputSpec.ToJson());
            Console.WriteLine("");    
        }

        static void Main(string[] args)
        {
            try
            {
                Program prg = new Program();
                prg.JsonSeralizeISpec();
                //prg.JsonSerializeTest();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }
    }
}
