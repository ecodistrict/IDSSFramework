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
