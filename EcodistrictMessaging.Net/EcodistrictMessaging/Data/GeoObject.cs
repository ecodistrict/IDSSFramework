using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Ecodistrict.Messaging.Data
{
    [DataContract]
    public class GeoObjects : List<GeoObject>
    {
       
    }

    [DataContract]
    public class GeoObject : Data
    {
        [DataMember]
        public string type { get; private set; }

        [DataMember]
        public string gml_id { get; private set; }

        [DataMember]
        public string kpiId { get; private set; }

        [DataMember]
        public double kpiValue { get; private set; }

        internal GeoObject() { }

        public GeoObject(string type, string gml_id, string kpiId, double kpiValue)
        {
            this.type = type;
            this.gml_id = gml_id;
            this.kpiId = kpiId;
            this.kpiValue = kpiValue;
        }
    }
}
