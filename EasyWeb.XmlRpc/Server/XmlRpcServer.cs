using System;
using LX.EasyWeb.XmlRpc.Serializer;

namespace LX.EasyWeb.XmlRpc.Server
{
    public abstract class XmlRpcServer : XmlRpcSystemHandlers
    {
        public XmlRpcServer()
        {
            TypeSerializerFactory = new TypeSerializerFactory();
            TypeConverterFactory = new TypeConverterFactory();

            XmlRpcServerConfig config = new XmlRpcServerConfig();
            config.EnabledForExtensions = true;
            Config = config;
        }

        public ITypeConverterFactory TypeConverterFactory { get; set; }

        public ITypeSerializerFactory TypeSerializerFactory { get; set; }

        public IXmlRpcServerConfig Config { get; set; }

        public Object Execute(IXmlRpcRequest request)
        {
            IXmlRpcHandler handler = GetHandler(request);
            return handler.Execute(request);
        }

        protected abstract IXmlRpcHandler GetHandler(IXmlRpcRequest request);
    }
}
