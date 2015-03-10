﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Ecodistrict.Messaging
{
    [DataContract]
    public class Atomic : Input
    {
        [DataMember]
        protected object value;

        public bool ShouldSerializevalue()
        {
            return value != null;
        }
    }
}