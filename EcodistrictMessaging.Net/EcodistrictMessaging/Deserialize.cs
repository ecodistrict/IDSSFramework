using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecodistrict.Messaging
{
    public static class Deserialize
    {
        static Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();

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
