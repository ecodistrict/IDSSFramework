using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Ecodistrict.Messaging
{  
    /// <summary>
    /// Data container containing the fundamentals for the geojson format.
    /// </summary>
    [DataContract]
    public class GeoJson : NonAtomic
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
        /// Create a new geojson object.
        /// </summary>
        /// <param name="label">Label to be visualized in the dashboard.</param>
        /// <param name="order">In what order this component should be visualized (comapred to other components)</param>
        public GeoJson(string label, object order = null)
        {
            this.type = "geojson";
            this.label = label;
            this.order = order;
        }

    }

    /// <summary>
    /// Collection of features for a geojson object.
    /// </summary>
    [DataContract]
    public class GeoValue : NonAtomic
    {
        /// <summary>
        /// What type of information. Defaultet to feature collection.
        /// </summary>
        [DataMember]
        public String type = "FeatureCollection";

        /// <summary>
        /// The geojson features.
        /// </summary>
        [DataMember]
        public Features features;

        internal GeoValue() { }
    }

    /// <summary>
    /// A collection of geojson features.
    /// </summary>
    [DataContract]
    public class Features : List<Feature>
    {

    }

    /// <summary>
    /// A geojson feature containing properties and geometry.
    /// </summary>
    [DataContract]
    public class Feature
    {
        /// <summary>
        /// Indication that this feature is a Feature (used for deserialization).
        /// </summary>
        [DataMember]
        String type = "Feature";

        /// <summary>
        /// Feature properties.
        /// </summary>
        [DataMember]
        public Dictionary<String, object> properties;

        /// <summary>
        /// Feature geometry.
        /// </summary>
        [DataMember]
        public Geometry geometry;
    }
    
    /// <summary>
    /// Type of geometry and it coordinates.
    /// </summary>
    [DataContract]
    public class Geometry
    {
        /// <summary>
        /// Type of geometry.
        /// </summary>
        [DataMember]
        public String type;
        
        /// <summary>
        /// The coordinates of the geometry.
        /// </summary>
        /// <remarks>
        /// The coordinates are here represented as a dynamic object which leaves a lot of 
        /// responsibility to the user to format this correctly. It's dynamic since there is 
        /// a lot of variations on how these coordinates can be represented. This may changed 
        /// to a more strict layout in the future.
        /// </remarks>
        [DataMember]
        public dynamic coordinates;
    }
}
