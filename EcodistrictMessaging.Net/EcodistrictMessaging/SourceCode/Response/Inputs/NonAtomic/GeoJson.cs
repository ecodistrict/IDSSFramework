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

        [DataMember]
        public GeoValue value;

        internal GeoJson() { }

        public GeoJson(string label, object order = null)
        {
            this.type = "geojson";
            this.label = label;
            this.order = order;
        }

    }

    [DataContract]
    public class GeoValue 
    {
        [DataMember]
        public Features features;
    }

    [DataContract]
    public class Features : List<Feature>
    {

    }

    [DataContract]
    public class Feature
    {
        [DataMember]
        String type = "Feature";

        [DataMember]
        public Dictionary<String, object> properties;
    }
}
