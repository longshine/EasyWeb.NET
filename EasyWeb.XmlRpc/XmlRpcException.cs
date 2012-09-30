//
// LX.EasyWeb.XmlRpc.XmlRpcException.cs
//
// Authors:
//	Longshine He <longshinehe@users.sourceforge.net>
//
// Copyright (c) 2012 Longshine He
//
// This code is distributed in the hope that it will be useful,
// but WITHOUT WARRANTY OF ANY KIND.
//

using System;
using System.Runtime.Serialization;

namespace LX.EasyWeb.XmlRpc
{
    /// <summary>
    /// The exception that is thrown when an error occurs during XML-RPC processing.
    /// </summary>
#if (!COMPACT_FRAMEWORK)
    [Serializable]
#endif
    public class XmlRpcException : ApplicationException
    {
        public XmlRpcException()
        { }

        public XmlRpcException(String message)
            : this(0, message, null)
        { }

        public XmlRpcException(String message, Exception innerException)
            : this(0, message, innerException)
        { }

        public XmlRpcException(Int32 faultCode, String faultString, Exception innerException)
            : base("Server returned a fault exception: [" + faultCode + "] " + faultString, innerException)
        {
            Fault = new XmlRpcFault(faultCode, faultString, innerException);
        }

        public IXmlRpcFault Fault { get; private set; }

#if (!COMPACT_FRAMEWORK)
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("fault", Fault);
            base.GetObjectData(info, context);
        }

        protected XmlRpcException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Fault = (XmlRpcFault)info.GetValue("fault", typeof(XmlRpcFault));
        }
#endif
    }
}
