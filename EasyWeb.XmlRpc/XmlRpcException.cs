using System;
using System.Runtime.Serialization;

namespace LX.EasyWeb.XmlRpc
{
    public class XmlRpcException : ApplicationException
    {
        public XmlRpcException()
        { }

        public XmlRpcException(String message)
            : base(message)
        { }

        public XmlRpcException(String message, Exception innerException)
            : base(message, innerException)
        { }

        protected XmlRpcException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
