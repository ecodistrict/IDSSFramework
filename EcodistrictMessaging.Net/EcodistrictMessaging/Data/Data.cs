using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Ecodistrict.Messaging.Data
{
    [DataContract]
    [Newtonsoft.Json.JsonConverter(typeof(DataItemConverter))]
    public class Data
    {        
        /// <summary>
        /// String representation of the type.
        /// </summary>
        [DataMember]
        public string type { get; protected set; }
    }

    public class DataItemConverter : JsonItemConverter<Data>
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object.
        /// </summary>
        /// <param name="objectType">type of object expected, one of the derived classes from <see cref="IMessage"/></param>
        /// <param name="jObject">contents of JSON object that will be deserialized</param>
        /// <returns>An empty derived <see cref="Output"/> object that can be filled with the json data.</returns>
        protected override Data Create(Type objectType, Newtonsoft.Json.Linq.JObject jObject)
        {
            var type = (string)jObject.Property("type");

            return new GeoObject();

            throw new ApplicationException(String.Format("The type {0} is not supported!", type));
        }
    }
}
