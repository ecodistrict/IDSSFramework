using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecodistrict.Messaging
{
    public static class Serialize
    {
        static Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();

        public static string Message(IMessage obj, bool indented = false)
        {
            if (indented)
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            else
                settings.Formatting = Newtonsoft.Json.Formatting.None;

            Type type = obj.GetDerivedType();
            if (type != null)
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, obj.GetDerivedType(), settings);
            else
                return ""; //TODO throw error ??
            
        }

        public static string InputSpecification(InputSpecification obj, bool indented = false)
        {
            if (indented)
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            else
                settings.Formatting = Newtonsoft.Json.Formatting.None;

            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, obj.GetType(), settings);
            

        }
    }
}
