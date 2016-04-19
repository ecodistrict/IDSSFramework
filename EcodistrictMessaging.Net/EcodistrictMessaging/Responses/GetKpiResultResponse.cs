using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Ecodistrict.Messaging.Data;

namespace Ecodistrict.Messaging.Responses
{
    [DataContract]
    public class GetKpiResultResponse : Response
    {
        [DataMember]
        protected string kpiId { get; private set; }

        [DataMember]
        protected double kpiValue { get; private set; }

        [DataMember]
        protected GeoObjects kpiValueList { get; private set; }
    }
}
