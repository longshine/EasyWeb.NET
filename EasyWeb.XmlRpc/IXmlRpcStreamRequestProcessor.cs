//
// LX.EasyWeb.XmlRpc.IXmlRpcStreamRequestProcessor.cs
//
// Authors:
//	Longshine He <longshinehe@users.sourceforge.net>
//
// Copyright (c) 2012 Longshine He
//
// This code is distributed in the hope that it will be useful,
// but WITHOUT WARRANTY OF ANY KIND.
//

namespace LX.EasyWeb.XmlRpc
{
    public interface IXmlRpcStreamRequestProcessor
    {
        void Execute(IXmlRpcStreamRequestConfig config, IServerStream stream);
    }
}
