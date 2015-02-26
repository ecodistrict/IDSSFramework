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
            GetModules,
            SelectModel,
            StartModel
        }

        public enum EType
        {
            Request,
            Response,
            Result
        }

        public static iMessage ParseJsonMessage(string message)
        {
           return new GetModulesMessageRequest();
        }
    }
}
