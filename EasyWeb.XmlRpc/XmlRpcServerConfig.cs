//
// LX.EasyWeb.XmlRpc.XmlRpcServerConfig.cs
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
    public class XmlRpcServerConfig : XmlRpcConfig, IXmlRpcServerConfig, IXmlRpcHttpServerConfig
    {
        public Boolean KeepAliveEnabled { get; set; }

        public Boolean EnabledForExceptions { get; set; }
    }
}
