using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecodistrict.Messaging
{
    /// <summary>
    /// Generic base-class that can be used in order to create custom converters for deserializing json-strings 
    /// for equivocal classes. E.g. <see cref="IMessage"/> with it's custom converter <see cref="MessageItemConverter"/>.  
    /// </summary>
    /// <typeparam name="T">Targeted class to convert</typeparam>
    /// <seealso cref="MessageItemConverter"/><seealso cref="OutputItemConverter"/>
    public abstract class JsonItemConverter<T> : Newtonsoft.Json.Converters.CustomCreationConverter<T>
    {
        /// <summary>
        /// Dummy creation of an instance of objectType, where no JSON object is available.
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override T Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object.
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">contents of JSON object that will be deserialized</param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, Newtonsoft.Json.Linq.JObject jObject);

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="Newtonsoft.Json.JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            // Load JObject from stream
            Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="Newtonsoft.Json.JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
