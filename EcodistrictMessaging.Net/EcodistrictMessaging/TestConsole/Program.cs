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
        static void Main(string[] args)
        {
            try
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }
    }
}
