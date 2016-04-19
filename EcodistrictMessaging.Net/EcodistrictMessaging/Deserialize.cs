using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecodistrict.Messaging
{
    /// <summary> 
    /// Static class that can be used to deserialize json-strings into .Net <see cref="IMessage"/> types.
    /// </summary> 
    public static class Deserialize<T> where T : IMessage
    {
        static Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();

        /// <summary>
        /// Is used to deserialize a json-strings into a .Net <see cref="IMessage"/> type.
        /// </summary>
        /// <remarks>
        /// See <see cref="Ecodistrict.Messaging.IMessage"/> and its derived classes for what 
        /// types of messages that can be deserialized. Messages that does not conform with
        /// a correctly formated dashboard message will result in a thrown <exception cref="ApplicationException"/>.
        /// </remarks> 
        /// <param name="message">Json-string formated according to the ecodistrict messaging protocol 
        /// <see href="https://github.com/ecodistrict/IDSSFramework/wiki"/>
        /// </param>
        /// <returns>
        /// One of <see cref="IMessage"/>'s derived classes, e.g. <see cref="GetModulesRequest"/>, 
        /// <see cref="SelectModuleResponse"/>,...
        /// </returns>
        public static T JsonString(string message)
        {
           try
           {
               T obj = (T)Newtonsoft.Json.JsonConvert.DeserializeObject(message, typeof(T));

               if (obj != null)
                   return obj;
               else
                   throw new Exception("Could not deserialize object", new Exception(message));
           }
           catch (System.Exception ex)
           {
               throw ex;
           }
        }        
    }

    public static class DeserializeData<T> where T : GeoValue
    {
        static Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();

        /// <summary>
        /// Is used to deserialize a json-strings into a .Net <see cref="IMessage"/> type.
        /// </summary>
        /// <remarks>
        /// See <see cref="Ecodistrict.Messaging.IMessage"/> and its derived classes for what 
        /// types of messages that can be deserialized. Messages that does not conform with
        /// a correctly formated dashboard message will result in a thrown <exception cref="ApplicationException"/>.
        /// </remarks> 
        /// <param name="message">Json-string formated according to the ecodistrict messaging protocol 
        /// <see href="https://github.com/ecodistrict/IDSSFramework/wiki"/>
        /// </param>
        /// <returns>
        /// One of <see cref="IMessage"/>'s derived classes, e.g. <see cref="GetModulesRequest"/>, 
        /// <see cref="SelectModuleResponse"/>,...
        /// </returns>
        public static T JsonString(string message)
        {
            try
            {
                T obj = (T)Newtonsoft.Json.JsonConvert.DeserializeObject(message, typeof(T));

                if (obj != null)
                    return obj;
                else
                    throw new Exception("Could not deserialize object", new Exception(message));
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }




}
