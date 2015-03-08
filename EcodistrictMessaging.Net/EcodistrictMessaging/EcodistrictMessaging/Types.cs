using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecodistrict.Messaging
{
    public static class Types
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
}
