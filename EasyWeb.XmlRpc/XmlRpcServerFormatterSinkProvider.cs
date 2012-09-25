using System;
using System.Runtime.Remoting.Channels;

namespace LX.EasyWeb.XmlRpc
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
            throw new NotImplementedException();
        }
    }
}
