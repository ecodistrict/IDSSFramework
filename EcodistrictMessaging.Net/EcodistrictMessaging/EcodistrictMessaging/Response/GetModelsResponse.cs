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
    class GetModelsResponse : Response
    {
        [DataMember]
        protected string name;
        [DataMember]
        protected string description;
        [DataMember]
        protected List<string> kpiList = new List<string>();

        public GetModelsResponse(string method, string type, string name, string moduleId, 
            string description, List<string> kpiList)
        {
            this.method = method;
            this.type = type;
            this.name = name;
            this.description = description;
            this.moduleId = moduleId;
            this.kpiList = kpiList;

        }
            

    }
}
