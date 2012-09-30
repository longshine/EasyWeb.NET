//
// LX.EasyWeb.XmlRpc.XmlRpcFault.cs
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

namespace LX.EasyWeb.XmlRpc
{
#if (!COMPACT_FRAMEWORK)
    [Serializable]
#endif
#if DEBUG
    public
#endif
    class XmlRpcFault : IXmlRpcFault
    {
        private Int32 _faultCode;
        private String _faultString;
        private Exception _exception;

        public XmlRpcFault(Int32 faultCode, String faultString)
            : this(faultCode, faultString, null)
        { }

        public XmlRpcFault(Int32 faultCode, String faultString, Exception exception)
        {
            _faultCode = faultCode;
            _faultString = faultString;
            _exception = exception;
        }

        [XmlRpcMember(Name = XmlRpcSpec.FAULT_CODE_TAG)]
        public Int32 FaultCode
        {
            get { return _faultCode; }
        }

        [XmlRpcMember(Name = XmlRpcSpec.FAULT_STRING_TAG)]
        public String FaultString
        {
            get { return _faultString; }
        }

        [XmlRpcIgnore]
        public Exception Exception
        {
            get { return _exception; }
        }
    }
}
