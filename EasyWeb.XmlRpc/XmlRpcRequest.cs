//
// LX.EasyWeb.XmlRpc.XmlRpcRequest.cs
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
#if DEBUG
    public
#endif
    class XmlRpcRequest : IXmlRpcRequest
    {
        public XmlRpcRequest()
        { }

        public XmlRpcRequest(String method, Object[] parameters)
        {
            MethodName = method;
            Parameters = parameters;
        }

        public String MethodName { get; private set; }
        public Object[] Parameters { get; private set; }
        public Object Target { get; set; }
    }
}
