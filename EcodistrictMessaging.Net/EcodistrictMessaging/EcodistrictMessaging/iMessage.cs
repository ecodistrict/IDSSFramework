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
    public class IMessage // : IConvertible
    {
        [DataMember]
        public string method { get; protected set; }
        [DataMember]
        public string type { get; protected set; }

        MessageGlobals.EMethod eMethod
        {
            get
            {
                switch (method)
                {
                    case "getModels":
                        return MessageGlobals.EMethod.GetModels;
                    case "selectModel":
                        return MessageGlobals.EMethod.SelectModel;
                    case "startModel":
                        return MessageGlobals.EMethod.StartModel;
                    case "modelResult":
                        return MessageGlobals.EMethod.ModelResult;
                    default:
                        return MessageGlobals.EMethod.NoMethod;
                }
            }
        }

        MessageGlobals.EType eType
        {
            get
            {
                switch (type)
                {
                    case "request":
                        return MessageGlobals.EType.Request;
                    case "response":
                        return MessageGlobals.EType.Response;
                    case "result":
                        return MessageGlobals.EType.Result;
                    default:
                        return MessageGlobals.EType.NoType;
                }
            }
        }

        public Type GetDerivedType()
        {

            switch(eType)
            {
                case MessageGlobals.EType.Request:
                    switch(eMethod)
                    {
                        case MessageGlobals.EMethod.GetModels:
                            return typeof(GetModelsRequest);
                        case MessageGlobals.EMethod.SelectModel:
                            return typeof(SelectModelRequest);
                        case MessageGlobals.EMethod.StartModel:
                            return typeof(StartModelRequest);
                    }
                    break;
                case MessageGlobals.EType.Response:
                    switch (eMethod)
                    {
                        case MessageGlobals.EMethod.GetModels:
                            return typeof(GetModelsResponse);
                        case MessageGlobals.EMethod.SelectModel:
                            return typeof(SelectModelResponse);
                        case MessageGlobals.EMethod.StartModel:
                            return typeof(StartModelResponse);
                    }
                    break;
                case MessageGlobals.EType.Result:
                    switch (eMethod)
                    {
                        case MessageGlobals.EMethod.ModelResult:
                            return typeof(ModelResult);
                    }
                    break;
            }


            return null;
        }

        //MessageGlobals.EMethod Method { get; set; }
        //MessageGlobals.EType Type { get; set; }

        //string ToJsonMessage();

        //#region IConvertible Members

        //[Pure]
        //TypeCode IConvertible.GetTypeCode()
        //{
        //    throw new NotImplementedException();
        //}

        //bool IConvertible.ToBoolean(IFormatProvider provider)
        //{
        //    throw new NotImplementedException();
        //}

        //char IConvertible.ToChar(IFormatProvider provider)
        //{
        //    throw new NotImplementedException();
        //}

        //sbyte IConvertible.ToSByte(IFormatProvider provider)
        //{
        //    throw new NotImplementedException();
        //}

        //byte IConvertible.ToByte(IFormatProvider provider)
        //{
        //    throw new NotImplementedException();
        //}

        //short IConvertible.ToInt16(IFormatProvider provider)
        //{
        //    throw new NotImplementedException();
        //}

        //ushort IConvertible.ToUInt16(IFormatProvider provider)
        //{
        //    throw new NotImplementedException();
        //}

        //int IConvertible.ToInt32(IFormatProvider provider)
        //{
        //    throw new NotImplementedException();
        //}

        //uint IConvertible.ToUInt32(IFormatProvider provider)
        //{
        //    throw new NotImplementedException();
        //}

        //long IConvertible.ToInt64(IFormatProvider provider)
        //{
        //    throw new NotImplementedException();
        //}

        //ulong IConvertible.ToUInt64(IFormatProvider provider)
        //{
        //    throw new NotImplementedException();
        //}

        //float IConvertible.ToSingle(IFormatProvider provider)
        //{
        //    throw new NotImplementedException();
        //}

        //double IConvertible.ToDouble(IFormatProvider provider)
        //{
        //    throw new NotImplementedException();
        //}

        //Decimal IConvertible.ToDecimal(IFormatProvider provider)
        //{
        //    throw new NotImplementedException();
        //}

        //DateTime IConvertible.ToDateTime(IFormatProvider provider)
        //{
        //    throw new NotImplementedException();
        //}

        //[Pure]
        //String IConvertible.ToString(IFormatProvider provider)
        //{
        //    Contract.Ensures(Contract.Result<string>() != null);
        //    throw new NotImplementedException();
        //}

        //Object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion


    }
}
