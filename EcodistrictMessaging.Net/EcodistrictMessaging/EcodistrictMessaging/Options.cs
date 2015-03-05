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
    public class Options : List<Option>
    {
    }

    [DataContract]
    public class Option
    {
        [DataMember]
        string value;
        [DataMember]
        string label;

        public Option(string value, string label)
        {
            this.value = value;
            this.label = label;
        }

        //// Implement this method to serialize data. The method is called  
        //// on serialization. 
        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    // Use the AddValue method to specify serialized values.
        //    info.AddValue("value", value, typeof(string));

        //}
    }
}
