using System;

namespace LX.EasyWeb.XmlRpc.Server
{
    interface IXmlRpcHandlerMapping
    {
        IXmlRpcHandler GetHandler(String name);
    }
}
