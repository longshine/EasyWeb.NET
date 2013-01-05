//
// LX.EasyWeb.XmlRpc.XmlRpcResponse.cs
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
    public class XmlRpcResponse : IXmlRpcResponse
    {
        public XmlRpcResponse()
        { }

        public XmlRpcResponse(Object result)
        {
            Result = result;
        }

        public Object Result { get; set; }

        public IXmlRpcFault Fault { get; set; }
    }
}
