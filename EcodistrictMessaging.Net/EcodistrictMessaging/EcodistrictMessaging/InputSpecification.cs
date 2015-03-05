using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Dynamic;
using System.Collections;
using System.Collections.Specialized;

namespace Ecodistrict.Messaging
{
    public class InputSpecification
    {
         dynamic inputs { get; set; }

         public InputSpecification()
        {
            inputs = new OrderedDictionary();
        }

        public void Add(string key, Input item)
         {
             inputs.Add("\"" + key + "\"", item);
         }

        public string ToJson()
        {
            string json = "";

            IDictionaryEnumerator myEnumerator = inputs.GetEnumerator();

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

        //public string ToJson()
        //{
        //    var settings = new DataContractJsonSerializerSettings();
        //    settings.EmitTypeInformation = EmitTypeInformation.Never;
        //    MemoryStream stream1 = new MemoryStream();
        //    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(SerializableDynamicObject), settings);

        //    ser.WriteObject(stream1, this);
        //    stream1.Position = 0;
        //    StreamReader sr = new StreamReader(stream1);
        //    string json = sr.ReadToEnd();
        //    //Newtonsoft.Json.JsonConvert.SerializeObject(dynamicProperties)
        //    return json;
        //}
    }
}
