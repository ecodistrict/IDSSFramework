using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecodistrict.Messaging
{

    /// <summary> 
    /// Static class that can be used to deserialize json-strings to .Net object types.
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
        /// <param name="message">Json-string formated according to the ecodistrict messaging protocol <see cref="https://github.com/ecodistrict/IDSSFramework/wiki"/></param>
        /// <param name="indented">Indication whether the string is indented or not (the dashboard send unindented strings since json ignore whitespaces).</param>
        /// <returns></returns>
        public static IMessage JsonMessage(string message, bool indented = false)
        {
            if(indented)
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            else
                settings.Formatting = Newtonsoft.Json.Formatting.None;

            IMessage messageObj = (IMessage)Newtonsoft.Json.JsonConvert.DeserializeObject(message, typeof(IMessage), settings);
            Type type = messageObj.GetDerivedType();

            IMessage obj;
            if (type != null)
                obj = (IMessage)Newtonsoft.Json.JsonConvert.DeserializeObject(message, type, settings);
            else
                obj = null;

            return obj;
        }
    }
}
