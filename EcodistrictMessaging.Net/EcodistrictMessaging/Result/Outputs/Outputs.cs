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

    public class OutputItemConverter : JsonItemConverter<Output>
    {
        public override Output Create(Type objectType)
        {
            throw new NotImplementedException();
        }

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
