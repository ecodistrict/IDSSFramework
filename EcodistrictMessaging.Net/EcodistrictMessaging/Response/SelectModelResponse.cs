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
    public class SelectModelResponse : Response
    {
        [DataMember]
        private string kpiId;
        [DataMember]
        private string variantId;
        [DataMember]
        private InputSpecification inputSpecification;

        /// <summary>
        /// Defines the response to the "selectModel" request sent by the dashboard.
        /// Can be seralized to a json-string that can be interpeted by the dashboard.
        /// </summary>
        /// <param name="moduleId">Unique identifer of the module using the messaging protokoll.</param>
        /// <param name="variantId">Used by dashboard for tracking.</param>
        /// <param name="kpiId">The kpi that the dashboard previously selected.</param>
        /// <param name="inputSpecification">The specification to the dashboard on what data the model need.</param>
        public SelectModelResponse(string moduleId, string variantId, string kpiId, InputSpecification inputSpecification)
        {
            this.method = "selectModel";
            this.type = "response";
            this.moduleId = moduleId;
            this.kpiId = kpiId;
            this.variantId = variantId;
            this.inputSpecification = inputSpecification;

        }
    }
}