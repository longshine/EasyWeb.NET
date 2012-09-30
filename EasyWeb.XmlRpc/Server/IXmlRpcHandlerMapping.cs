//
// LX.EasyWeb.XmlRpc.Server.IXmlRpcHandlerMapping.cs
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

namespace LX.EasyWeb.XmlRpc.Server
{
    interface IXmlRpcHandlerMapping
    {
        IXmlRpcHandler GetHandler(String name);
    }
}
