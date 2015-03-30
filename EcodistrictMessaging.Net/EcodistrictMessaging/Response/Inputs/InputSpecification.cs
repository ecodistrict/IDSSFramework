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
    /// The collection of <see cref="Input"/>s the module need in order to calculate a given kpi.
    /// </summary>
    /// <seealso cref="Atomic"/> <seealso cref="NonAtomic"/>
    [DataContract]
    public class InputSpecification : Dictionary<string, Input>
    {

    }
}
