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
    public class GeoJson : NonAtomic
    {
        [DataMember]
        string geometryObject = "polygon";

        internal GeoJson() { }

        public GeoJson(string label, object order = null)
        {
            this.type = "geojson";
            this.label = label;
            this.order = order;
        }

    }
}
