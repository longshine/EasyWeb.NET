using System;

namespace LX.EasyWeb.XmlRpc.Server
{
    interface IXmlRpcOverloadedHandlerMapping : IXmlRpcHandlerMapping
    {
        IXmlRpcHandler GetHandler(String name, Object[] args);
    }
}
