using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Ecodistrict.Messaging.Output
{
    [DataContract]
    public class GeoJson : Output
    {
        [DataMember]
        string geometryObject = "polygon";

        [DataMember]
        public GeoValue value;

        internal GeoJson()
        {
            this.type = "geojson";
        }

        public GeoJson(Ecodistrict.Messaging.GeoJson geojson)
        {
            this.type = "geojson";
            value = geojson.value;
        }
    }
}
