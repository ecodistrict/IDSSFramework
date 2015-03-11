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
        protected string value;
        [DataMember]
        protected string label;

        public Option(string value, string label)
        {
            this.value = value;
            this.label = label;
        }
    }
}
