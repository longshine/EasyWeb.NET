using System;

namespace LX.EasyWeb.XmlRpc.Server
{
    public abstract class XmlRpcServer : XmlRpcSystemHandlers
    {
        public XmlRpcServer()
        {
            TypeFactory = new TypeFactory();
            Config = new XmlRpcServerConfig();
            TypeConverterFactory = new TypeConverterFactory();
        }

        public ITypeConverterFactory TypeConverterFactory { get; set; }

        public ITypeFactory TypeFactory { get; set; }

        public IXmlRpcServerConfig Config { get; set; }

        public Object Execute(IXmlRpcRequest request)
        {
            IXmlRpcHandler handler = GetHandler(request);
            return handler.Execute(request);
        }

        protected abstract IXmlRpcHandler GetHandler(IXmlRpcRequest request);
    }
}
