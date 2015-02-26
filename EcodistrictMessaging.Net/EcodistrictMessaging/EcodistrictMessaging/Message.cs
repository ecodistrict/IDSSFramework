using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecodistrict.Messaging
{
    public interface iMessage
    {
        MessageGlobals.EMethod Method { get; set; }
        MessageGlobals.EType Type { get; set; }

        string ToJsonMessage();


    }
}
