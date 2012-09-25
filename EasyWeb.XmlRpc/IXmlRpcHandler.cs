using System;
using System.Reflection;
using LX.EasyWeb.XmlRpc.Server;

namespace LX.EasyWeb.XmlRpc
{
    /// <summary>
    /// Represents a RPC handler.
    /// </summary>
    public interface IXmlRpcHandler
    {
        /// <summary>
        /// Performs the request and returns the result object.
        /// </summary>
        /// <param name="request">the request being performed</param>
        /// <returns>the result object</returns>
        Object Execute(IXmlRpcRequest request);
    }

    interface IOverloadXmlRpcHandler : IXmlRpcHandler
    {
        IXmlRpcHandler GetHandler(String name, Object[] args);
    }

    interface IXmlRpcMetaDataHandler : IXmlRpcHandler
    {
        String[][] Signatures { get; }
        String MethodHelp { get; }
    }

    class XmlRpcHandler : IXmlRpcHandler
    {
        private Type _type;
        private IXmlRpcTargetProvider _targetProvider;

        public XmlRpcHandler(Type type, MethodInfo mi, ITypeConverterFactory typeConverterFactory, IXmlRpcTargetProvider provider)
        {
            _type = type;
            Method = mi;
            ParameterInfo[] pis = mi.GetParameters();
            TypeConverters = new ITypeConverter[pis.Length];
            for (Int32 i = 0; i < pis.Length; i++)
            {
                TypeConverters[i] = typeConverterFactory.GetTypeConverter(pis[i].ParameterType);
            }
            _targetProvider = provider;
        }

        public MethodInfo Method { get; private set; }

        public ITypeConverter[] TypeConverters { get; private set; }

        public Object Execute(IXmlRpcRequest request)
        {
            return Method.Invoke(_targetProvider == null ? request.Target : _targetProvider.GetTarget(request), request.Parameters);
        }
    }
}
