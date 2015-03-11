using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecodistrict.Messaging
{
    public static class MessageTypes
    {
        public enum MMethod
        {
            GetModels,
            SelectModel,
            StartModel,
            ModelResult,
            NoMethod
        }

        public enum MType
        {
            Request,
            Response,
            Result,
            NoType
        }
    }

    public enum ModelStatus
    {
        Processing,
        Success
    }
}
