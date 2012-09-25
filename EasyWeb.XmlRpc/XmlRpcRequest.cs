using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace LX.EasyWeb.XmlRpc
{
    public class XmlRpcRequest : IXmlRpcRequest
    {
        public XmlRpcRequest()
        { }

        public XmlRpcRequest(String method, Object[] parameters)
        {
            Method = method;
            Parameters = parameters;
        }

        public String Method { get; private set; }
        public Object[] Parameters { get; private set; }
        public MethodInfo MethodInfo { get; private set; }
        public Guid ProxyId { get; private set; }
        public String XmlRpcMethod { get; private set; }
        public Object Target { get; set; }
    }
}
