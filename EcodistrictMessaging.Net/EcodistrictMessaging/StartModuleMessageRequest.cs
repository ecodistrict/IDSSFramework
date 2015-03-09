using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecodistrict.Messaging
{
    public class GetModulesMessageRequest: iMessage
    {
        

        public string VariantId { get; set; }
        public string ModuleId { get; set; }
        public string kpiAlias { get; set; }


        public MessageGlobals.EMethod Method
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public MessageGlobals.EType Type
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ToJsonMessage()
        {
            throw new NotImplementedException();
        }
    }
}
