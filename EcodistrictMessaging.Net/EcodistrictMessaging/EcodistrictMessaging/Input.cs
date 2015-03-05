using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using System.Collections.Specialized;

namespace Ecodistrict.Messaging
{
    public class Input
    {
        protected dynamic inputs = new OrderedDictionary();

        public virtual string ToJson()
        {
            throw new NotImplementedException();
        }
    }
}
