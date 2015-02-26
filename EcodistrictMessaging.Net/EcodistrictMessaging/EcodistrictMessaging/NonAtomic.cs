using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Ecodistrict.Messaging
{
    [DataContract]
    public class NonAtomic : Input
    {
        [DataMember]
        protected List<Input> inputs = new List<Input>();

        public virtual void Add(Input item) 
        {
            throw new NotImplementedException();
        }

    }
}
