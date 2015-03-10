﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Ecodistrict.Messaging
{
    /// <summary> 
    /// Base class to all messagingtypes that can be sent from the dashboard.
    /// </summary> 
    [DataContract]
    public class Request : IMessage
    {

    }
}