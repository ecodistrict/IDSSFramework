using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Ecodistrict.Messaging
{
    [DataContract]
    public class List : NonAtomic
    {
        public List(string label)
        {
            inputs.Add("\"type\"", "list");
            inputs.Add("\"label\"", label);
        }


        //public override void Add(string key, Input item)
        //{
        //    inputs.Add(key, item);
        //}
    }
}
