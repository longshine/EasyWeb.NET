//
// LX.EasyWeb.XmlRpc.Server.IXmlRpcHandlerMapping.cs
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
using System.Reflection;

namespace LX.EasyWeb.XmlRpc.Server
{
    class ReflectiveXmlRpcHandler : IXmlRpcHandler
    {
        private AbstractReflectiveHandlerMapping _mapping;
        private readonly XmlRpcMethod[] _methods;
        private Type _type;
        private IXmlRpcTargetProvider _targetProvider;

        public ReflectiveXmlRpcHandler(AbstractReflectiveHandlerMapping mapping, ITypeConverterFactory typeConverterFactory, Type type, IXmlRpcTargetProvider provider, MethodInfo[] methods)
        {
            _mapping = mapping;
            _type = type;
            _methods = new XmlRpcMethod[methods.Length];
            for (Int32 i = 0; i < methods.Length; i++)
            {
                _methods[i] = new XmlRpcMethod(methods[i], typeConverterFactory);
            }
            _targetProvider = provider;
        }

        public Object Execute(IXmlRpcRequest request)
        {
            if (_mapping.AuthenticationHandler != null && !_mapping.AuthenticationHandler.IsAuthorized(request))
                throw new XmlRpcException("Not authorized");
            XmlRpcMethod method = GetMethod(request.Parameters);
            ConvertParams(method.TypeConverters, request.Parameters);
            return Invoke(method.Method, _targetProvider == null ? request.Target : _targetProvider.GetTarget(request), request.Parameters);
        }

        public MethodInfo GetMethod(IXmlRpcRequest request)
        {
            return GetMethod(request.Parameters).Method;
        }

        private static void ConvertParams(ITypeConverter[] converters, Object[] args)
        {
            for (Int32 i = 0; i < args.Length; i++)
                args[i] = converters[i].Convert(args[i]);
        }

        private Object Invoke(MethodInfo methodInfo, Object target, Object[] parameters)
        {
            try
            {
                return methodInfo.Invoke(target, parameters);
            }
            catch (Exception ex)
            {
                throw new XmlRpcException("Failed to invoke method " + methodInfo.Name + ": " + ex.Message, ex);
            }
        }

        private XmlRpcMethod GetMethod(Object[] args)
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
                        return m;
                }
            }
            throw new XmlRpcException("No method matching arguments: " + Util.GetSignature(args));
        }

        class XmlRpcMethod
        {
            public MethodInfo Method;
            public ITypeConverter[] TypeConverters;

            public XmlRpcMethod(MethodInfo mi, ITypeConverterFactory typeConverterFactory)
            {
                Method = mi;
                ParameterInfo[] pis = mi.GetParameters();
                TypeConverters = new ITypeConverter[pis.Length];
                for (Int32 i = 0; i < pis.Length; i++)
                {
                    TypeConverters[i] = typeConverterFactory.GetTypeConverter(pis[i].ParameterType);
                }
            }
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
