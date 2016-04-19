using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecodistrict.Messaging.Data
{
    public class Output
    {
        public Output(string kpiId, double? kpiValue = null)
        {
            this.KpiId = kpiId;
            this.KpiValue = kpiValue;
        }


        public string KpiId { get; private set; }
        public double? KpiValue { get; private set; }
    }

    public class OutputDetailed
    {
        public OutputDetailed(string kpiId)
        {
            this.KpiId = kpiId;
        }


        public string KpiId { get; private set; }
        GeoObjects _kpiValueList;
        public GeoObjects KpiValueList
        {
            get
            {
                if (_kpiValueList == null)
                    _kpiValueList = new GeoObjects();

                return _kpiValueList;
            }
            set
            {
                _kpiValueList = value;
            }
        }
    }
}
