//
// LX.EasyWeb.XmlRpc.Server.XmlRpcServerFormatterSinkProvider.cs
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
using System.Runtime.Remoting.Channels;

namespace LX.EasyWeb.XmlRpc.Server
{
    public class XmlRpcServerFormatterSinkProvider : IServerFormatterSinkProvider
    {
        public IServerChannelSinkProvider Next { get; set; }

        public IServerChannelSink CreateSink(IChannelReceiver channel)
        {
            IServerChannelSink sink = null;
            if (Next != null)
                sink = Next.CreateSink(channel);
            return new XmlRpcServerChannelSink(sink);
        }

        public void GetChannelData(IChannelDataStore channelData)
        {
            // TODO: not required???
        }
    }
}
