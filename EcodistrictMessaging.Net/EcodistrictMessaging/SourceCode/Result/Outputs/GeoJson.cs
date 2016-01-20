using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Ecodistrict.Messaging.Output
{
    /// <summary>
    /// Data container containing the fundamentals for the geojson format.
    /// </summary>
    [DataContract]
    public class GeoJson : Output
    {
        /// <summary>
        /// Specifies the type of the geojson geometry.
        /// </summary>
        [DataMember]
        string geometryObject = "polygon";

        /// <summary>
        /// Contains the geojson data (properties and geometry)
        /// </summary>
        [DataMember]
        public GeoValue value;

        /// <summary>
        /// Default constructor for deserialization.
        /// </summary>
        internal GeoJson()
        {
            this.type = "geojson";
        }

        /// <summary>
        /// Copy-constructor used to convert the <see cref="Input"/> version of the <see cref="Ecodistrict.Messaging.GeoJson"/>
        /// to the <see cref="Ecodistrict.Messaging.Output.Output"/> version of the <see cref="Ecodistrict.Messaging.GeoJson"/>
        /// </summary>
        /// <param name="geojson"></param>
        public GeoJson(Ecodistrict.Messaging.GeoJson geojson)
        {
            this.type = "geojson";
            value = geojson.value;
        }
    }
}
