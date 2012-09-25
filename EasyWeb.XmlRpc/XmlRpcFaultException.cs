using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace LX.EasyWeb.XmlRpc
{
#if (!COMPACT_FRAMEWORK)
    [Serializable]
#endif
    public class XmlRpcFaultException : XmlRpcException
    {
        public XmlRpcFaultException(Int32 faultCode, String faultString)
            : base("Server returned a fault exception: [" + faultCode + "] " + faultString)
        {
            FaultCode = faultCode;
            FaultString = faultString;
        }

        public Int32 FaultCode { get; private set; }

        public String FaultString { get; private set; }

#if (!COMPACT_FRAMEWORK)
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("faultCode", FaultCode);
            info.AddValue("faultString", FaultString);
            base.GetObjectData(info, context);
        }

        protected XmlRpcFaultException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            FaultCode = (Int32)info.GetValue("faultCode", typeof(Int32));
            FaultString = (String)info.GetValue("faultString", typeof(String));
        }
#endif
    }
}
