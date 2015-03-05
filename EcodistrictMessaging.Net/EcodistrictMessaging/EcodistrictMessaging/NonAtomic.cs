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
    public class NonAtomic : Input
    {
        public void Add(string key, Input item)
        {
            inputs.Add("\"" + key + "\"", item);
        }

        public override string ToJson()
        {
            string jsonHeader = "";
            string json = "";

            IDictionaryEnumerator myEnumerator = inputs.GetEnumerator();

            bool done = !myEnumerator.MoveNext();

            //Get header information
            while (!done)
            {
                Type baseType = myEnumerator.Value.GetType().BaseType;
                if ((baseType != typeof(Atomic)) & (baseType != typeof(NonAtomic)))
                    jsonHeader += string.Format("{0}:{1}", myEnumerator.Key, Newtonsoft.Json.JsonConvert.SerializeObject(myEnumerator.Value));
                else
                    break;

                done = !myEnumerator.MoveNext();
                if (!done)
                    jsonHeader += ",";
            }

            //Get inputs
            while (!done)
            {
                Type baseType = myEnumerator.Value.GetType().BaseType;
                if ((baseType == typeof(Atomic)) | (baseType == typeof(NonAtomic)))
                    json += string.Format("{0}:{1}", myEnumerator.Key, ((Input)myEnumerator.Value).ToJson());
                else
                    throw new Exception("Input::NonAtomic Implementation error!");

                done = !myEnumerator.MoveNext();
                if (!done)
                    json += ",";
            }

            json = string.Format("{0}{1}{2}", "{", json, "}");
            //while (!done)
            //{
            //    if (myEnumerator.Value.GetType() != typeof(Input))
            //        jsonHeader += string.Format("{0}:{1}", myEnumerator.Key, Newtonsoft.Json.JsonConvert.SerializeObject(myEnumerator.Value));
            //    else
            //        json += string.Format("{0}:{1}", myEnumerator.Key, ((Input)myEnumerator.Value).ToJson());

            //    done = !myEnumerator.MoveNext();
            //    if (!done)
            //        json += ",";
            //}

            json = string.Format("{0}{1}{2}{3}{4}", "{", jsonHeader, "\"inputs\":", json, "}");
            return json;
        }


        //[DataMember]
        //protected List<Input> inputs = new List<Input>();

        //[DataMember]
        //protected SerializableDynamicObject inputs = new SerializableDynamicObject();

        //public virtual void Add(string key, Input item) 
        //{
        //    throw new NotImplementedException();
        //}

    }
}
