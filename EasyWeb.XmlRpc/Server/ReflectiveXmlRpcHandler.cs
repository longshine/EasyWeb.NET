using System;
using System.Reflection;

namespace LX.EasyWeb.XmlRpc.Server
{
    class ReflectiveXmlRpcHandler : IOverloadXmlRpcHandler
    {
        private AbstractReflectiveHandlerMapping _mapping;
        private readonly XmlRpcHandler[] _methods;
        private Type _type;

        public ReflectiveXmlRpcHandler(AbstractReflectiveHandlerMapping mapping, ITypeConverterFactory typeConverterFactory, Type type, IXmlRpcTargetProvider provider, MethodInfo[] methods)
        {
            _mapping = mapping;
            _type = type;
            _methods = new XmlRpcHandler[methods.Length];
            for (Int32 i = 0; i < methods.Length; i++)
            {
                _methods[i] = new XmlRpcHandler(_type, methods[i], typeConverterFactory, provider);
            }
            TargetProvider = provider;
        }

        public IXmlRpcTargetProvider TargetProvider { get; set; }

        public IXmlRpcHandler GetHandler(String name, Object[] args)
        {
            foreach (var m in _methods)
            {
                if (m.TypeConverters.Length == args.Length)
                {
                    Boolean match = true;
                    for (Int32 i = 0; i < args.Length; i++)
                    {
                        if (!m.TypeConverters[i].IsConvertable(args[i]))
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match)
                    {
                        for (Int32 i = 0; i < args.Length; i++)
                        {
                            args[i] = m.TypeConverters[i].Convert(args[i]);
                        }
                        return m;
                    }
                }
            }
            throw new XmlRpcException("No method matching arguments: " + Util.GetSignature(args));
        }

        public Object Execute(IXmlRpcRequest request)
        {
            if (_mapping.AuthenticationHandler != null && !_mapping.AuthenticationHandler.IsAuthorized(request))
                throw new XmlRpcException("Not authorized");
            IXmlRpcHandler handler = GetHandler(null, request.Parameters);
            return handler.Execute(request);
        }
    }

    class ReflectiveXmlRpcMetaDataHandler : ReflectiveXmlRpcHandler, IXmlRpcMetaDataHandler
    {
        public ReflectiveXmlRpcMetaDataHandler(AbstractReflectiveHandlerMapping mapping, ITypeConverterFactory typeConverterFactory, Type type,
            IXmlRpcTargetProvider provider, MethodInfo[] methods, String[][] signatures, String methodHelp)
            : base(mapping, typeConverterFactory, type, provider, methods)
        {
            Signatures = signatures;
            MethodHelp = methodHelp;
        }

        public String[][] Signatures { get; private set; }

        public String MethodHelp { get; private set; }
    }
}
