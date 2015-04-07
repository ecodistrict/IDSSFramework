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
    /// The base class for all types of inputs that can be used in the input specification
    /// <see cref="InputSpecification"/>.
    /// </summary>
    [DataContract]
    [Newtonsoft.Json.JsonConverter(typeof(InputItemConverter))]
    public class Input
    {
        /// <summary>
        /// Mandatory label, describing the component.
        /// </summary>
        /// <remarks>
        /// The label will be visualized next to the input selector described by this combonent. 
        /// E.g. <see cref="Number"/>, <see cref="Checkbox"/>, <see cref="List"/>, ...
        /// </remarks>
        [DataMember]
        protected string label;

        /// <summary>
        /// What typ of component the dashboard shall visualize, e.g. "number","text",'list',...
        /// </summary>
        [DataMember]
        protected string type;

        /// <summary>
        /// The order in which this component should be rendered in the dashboard (ascending order).
        /// Left out or null value will be interpeted as 0 in the dashboard.
        /// </summary>
        [DataMember]
        protected object order;

        /// <summary>
        /// If the class property "order" should be serialized with the Input object.
        /// </summary>
        /// <remarks>
        /// Is <see cref="Boolean">false</see> if <see cref="order"/> is omitted in the constructor.
        /// </remarks>
        /// <returns> 
        /// <see cref="Boolean">true</see> if the class property <see cref="order"/>  should be serialized with the <see cref="Input"/> object.
        /// </returns>
        public bool ShouldSerializeorder()
        {
            return order != null;
        }
    }


    public class InputItemConverter : JsonItemConverter<Input>//Newtonsoft.Json.Converters.CustomCreationConverter<IMessage>
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object.
        /// </summary>
        /// <param name="objectType">type of object expected, one of the derived classes from <see cref="IMessage"/></param>
        /// <param name="jObject">contents of JSON object that will be deserialized</param>
        /// <returns>An empty derived <see cref="Input"/> object that can be filled with the json data.</returns>
        protected override Input Create(Type objectType, Newtonsoft.Json.Linq.JObject jObject)
        {
            var type = (string)jObject.Property("type");

            switch (type)
            {
                //Atomic
                case "checkbox":
                    return new Checkbox();
                case "number":
                    return new Number();
                case "select":
                    return new Select();
                case "text":
                    return new Text();
                //NonAtomic
                case "inputGroup":
                    return new InputGroup();
                case "list":
                    return new List();

            }

            throw new ApplicationException(String.Format("The input type '{0}' is not supported!", type));
        }
    }
}
