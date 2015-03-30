using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecodistrict.Messaging
{

    /// <summary> 
    /// Static class that can be used to deserialize json-strings or json-byte-arrays to .Net object types.
    /// </summary> 
    public static class Deserialize
    {
        static Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();

        /// <summary>
        /// Is used to deserialize json-strings to .Net message types.
        /// </summary>
        /// <remarks>
        /// See <see cref="Ecodistrict.Messaging.IMessage"/> and its derived classes for what 
        /// types of messages that can be deserialized.
        /// </remarks> 
        /// <param name="message">Json-string formated according to the ecodistrict messaging protocol <see href="https://github.com/ecodistrict/IDSSFramework/wiki"/></param>
        /// <returns></returns>
        public static IMessage JsonString(string message)
        {
            IMessage messageObj = (IMessage)Newtonsoft.Json.JsonConvert.DeserializeObject(message, typeof(IMessage));
            Type type = messageObj.GetDerivedType();

            IMessage obj;
            if (type != null)
                obj = (IMessage)Newtonsoft.Json.JsonConvert.DeserializeObject(message, type, settings);
            else
                obj = null;

            return obj;
        }

        /// <summary>
        /// Is used to deserialize json-byte array to .Net message types.
        /// </summary>
        /// <remarks>
        /// See <see cref="Ecodistrict.Messaging.IMessage"/> and its derived classes for what 
        /// types of messages that can be deserialized.
        /// </remarks> 
        /// <param name="message">Json byte array formated according to the ecodistrict messaging protocol <see href="https://github.com/ecodistrict/IDSSFramework/wiki"/></param>
        /// <returns></returns>
        public static IMessage JsonByteArr(byte[] message)
        {
            string json = Encoding.UTF8.GetString(message);
            return JsonString(json);
        }
    }
}
