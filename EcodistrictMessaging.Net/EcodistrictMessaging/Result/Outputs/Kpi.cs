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
    public class Kpi : Output
    {
        [DataMember]
        protected string type;
        [DataMember]
        protected string value;
        [DataMember]
        protected string info;
        [DataMember]
        protected string unit;

        public Kpi(string value, string info, string unit)
        {
            this.type = "kpi";
            this.value = value;
            this.info = info;
            this.unit = unit;

        }

    }
}
