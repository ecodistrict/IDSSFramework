using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using System.Diagnostics.Contracts;
using System.Threading;

namespace Ecodistrict.Messaging
{
    [DataContract]
    /// Base class
    public class IMessage 
    {
        [DataMember]
        public string method { get; protected set; }
        [DataMember]
        public string type { get; protected set; }

        Types.MMethod eMethod
        {
            get
            {
                switch (method)
                {
                    case "getModels":
                        return Types.MMethod.GetModels;
                    case "selectModel":
                        return Types.MMethod.SelectModel;
                    case "startModel":
                        return Types.MMethod.StartModel;
                    case "modelResult":
                        return Types.MMethod.ModelResult;
                    default:
                        return Types.MMethod.NoMethod;
                }
            }
        }

        Types.MType eType
        {
            get
            {
                switch (type)
                {
                    case "request":
                        return Types.MType.Request;
                    case "response":
                        return Types.MType.Response;
                    case "result":
                        return Types.MType.Result;
                    default:
                        return Types.MType.NoType;
                }
            }
        }

        public Type GetDerivedType()
        {

            switch(eType)
            {
                case Types.MType.Request:
                    switch(eMethod)
                    {
                        case Types.MMethod.GetModels:
                            return typeof(GetModelsRequest);
                        case Types.MMethod.SelectModel:
                            return typeof(SelectModelRequest);
                        case Types.MMethod.StartModel:
                            return typeof(StartModelRequest);
                    }
                    break;
                case Types.MType.Response:
                    switch (eMethod)
                    {
                        case Types.MMethod.GetModels:
                            return typeof(GetModelsResponse);
                        case Types.MMethod.SelectModel:
                            return typeof(SelectModelResponse);
                        case Types.MMethod.StartModel:
                            return typeof(StartModelResponse);
                    }
                    break;
                case Types.MType.Result:
                    switch (eMethod)
                    {
                        case Types.MMethod.ModelResult:
                            return typeof(ModelResult);
                    }
                    break;
            }


            return null;
        }

    }
}
