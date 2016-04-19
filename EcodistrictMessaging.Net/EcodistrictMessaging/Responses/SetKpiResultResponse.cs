using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Ecodistrict.Messaging.Responses
{
    [DataContract]
    public class SetKpiResultResponse : Response
    {
        [DataMember]
        public string caseId { get; private set; }

        /// <summary>
        /// The variant id acquired from the dashboard in the <see cref="SelectModuleRequest"/> message.
        /// </summary>
        [DataMember]
        public string variantId { get; private set; }

        [DataMember]
        protected string kpiId { get; private set; }

        [DataMember]
        protected string status { get; private set; }

        [DataMember]
        protected string info { get; private set; }
    }
}
