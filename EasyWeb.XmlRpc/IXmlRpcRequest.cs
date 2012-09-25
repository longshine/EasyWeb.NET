using System;
using System.Reflection;

namespace LX.EasyWeb.XmlRpc
{
    public interface IXmlRpcRequest
    {
        String Method { get; }
        Object[] Parameters { get; }
        MethodInfo MethodInfo { get; }
        Guid ProxyId { get; }
        String XmlRpcMethod { get; }
        Object Target { get; set; }
    }
}
