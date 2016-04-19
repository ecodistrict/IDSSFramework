using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Ecodistrict.Messaging.Responses
{    
    /// <summary>
    /// Received data from data module
    /// </summary>
    [DataContract]
    public class GetDataResponse : Response
    {
        [DataMember]
        public string caseId { get; private set; }

        /// <summary>
        /// The variant id acquired from the dashboard in the <see cref="SelectModuleRequest"/> message.
        /// </summary>
        [DataMember]
        public string variantId { get; private set; }

        [DataMember]
        public string calculationId { get; private set; }

        //[DataMember]
        //public Dictionary<string,object> data { get; private set; }

        /// <summary>
        /// The data the module need to calculate the selected kpi.
        /// </summary>
        [DataMember]
        public Dictionary<string, object> data { get; private set; }
    }
}
