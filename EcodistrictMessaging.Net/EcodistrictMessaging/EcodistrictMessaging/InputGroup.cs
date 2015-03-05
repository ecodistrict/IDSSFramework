using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecodistrict.Messaging
{    
    class InputGroup : NonAtomic
    {
        public InputGroup(string label, object order = null)
        {
            inputs.Add("\"type\"", "inputGroup");
            inputs.Add("\"label\"", label);

            inputs.Add("\"order\"", order);
        }
    }
}
