using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecodistrict.Messaging
{
    public static class MessageGlobals
    {
        public enum EMethod
        {
            GetModels,
            SelectModel,
            StartModel,
            ModelResult,
            NoMethod
        }

        public enum EType
        {
            Request,
            Response,
            Result,
            NoType
        }

        public static object ParseJsonMessage(string message)
        {
            IMessage messageObj = (IMessage)Newtonsoft.Json.JsonConvert.DeserializeObject(message, typeof(IMessage));
            Type type = messageObj.GetDerivedType();

            object obj;
            if (type != null)
                obj = Newtonsoft.Json.JsonConvert.DeserializeObject(message, type);
            else
                obj = null;

            return obj;
        }
    }
}
