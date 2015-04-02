using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using Newtonsoft.Json;

namespace Ecodistrict.Messaging
{
    /// <summary>
    /// A list of <see cref="Output"/>s. Used by <see cref="ModuleResult"/>.
    /// </summary>
    [DataContract]
    public class Outputs : List<Output>
    {
    }

    /// <summary>
    /// The base class of all outputs that can be sent to the dashboard.
    /// </summary>
    [DataContract]
    [Newtonsoft.Json.JsonConverter(typeof(OutputItemConverter))]
    public class Output
    {
        /// <summary>
        /// An indicator to the dashboard on what type of module output that is received.
        /// </summary>
        [DataMember]
        protected string type;
    }

    /// <summary>
    /// A custom converter for deserializing json-strings to the equivocal class <see cref="Output"/> to
    /// its derived classes, e.g. <see cref="Kpi"/>.
    /// </summary>
    /// <remarks>
    /// Currently only one <see cref="Output"/> type is avaiable namely <see cref="Kpi"/>.
    /// </remarks>
    public class OutputItemConverter : JsonItemConverter<Output>
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object.
        /// </summary>
        /// <param name="objectType">type of object expected, one of the derived classes from <see cref="IMessage"/></param>
        /// <param name="jObject">contents of JSON object that will be deserialized</param>
        /// <returns>An empty derived <see cref="Output"/> object that can be filled with the json data.</returns>
        protected override Output Create(Type objectType, Newtonsoft.Json.Linq.JObject jObject)
        {
            var type = (string)jObject.Property("type");

            switch (type)
            {
                case "kpi":
                    return new Kpi();
            }

            throw new ApplicationException(String.Format("The output type {0} is not supported!", type));
        }
    }
}
