//
// LX.EasyWeb.XmlRpc.Server.XmlRpcServer.cs
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
using LX.EasyWeb.XmlRpc.Serializer;

namespace LX.EasyWeb.XmlRpc.Server
{
    /// <summary>
    /// XML-RPC server.
    /// </summary>
    public abstract class XmlRpcServer : XmlRpcSystemHandlers
    {
        /// <summary>
        /// Initializes.
        /// </summary>
        public XmlRpcServer()
        {
            TypeSerializerFactory = new TypeSerializerFactory();
            TypeConverterFactory = new TypeConverterFactory();

            XmlRpcServerConfig config = new XmlRpcServerConfig();
            config.EnabledForExtensions = true;
            Config = config;
        }

        /// <summary>
        /// Gets or set the <see cref="LX.EasyWeb.XmlRpc.ITypeConverterFactory"/>.
        /// </summary>
        public ITypeConverterFactory TypeConverterFactory { get; set; }

        /// <summary>
        /// Gets or set the <see cref="LX.EasyWeb.XmlRpc.ITypeSerializerFactory"/>.
        /// </summary>
        public ITypeSerializerFactory TypeSerializerFactory { get; set; }

        /// <summary>
        /// Gets or set the <see cref="LX.EasyWeb.XmlRpc.IXmlRpcServerConfig"/>.
        /// </summary>
        public IXmlRpcServerConfig Config { get; set; }

        /// <summary>
        /// Executes a XML-RPC request.
        /// </summary>
        /// <param name="request">the <see cref="LX.EasyWeb.XmlRpc.IXmlRpcRequest"/> to execute</param>
        /// <returns>a result object</returns>
        /// <exception cref="LX.EasyWeb.XmlRpc.XmlRpcException">no matched handler is found, or an exception occurs when handling the request</exception>
        public Object Execute(IXmlRpcRequest request)
        {
            IXmlRpcHandler handler = GetHandler(request);
            return handler.Execute(request);
        }

        /// <summary>
        /// Gets the <see cref="LX.EasyWeb.XmlRpc.IXmlRpcHandler"/> which the request called.
        /// </summary>
        /// <param name="request">the incoming <see cref="LX.EasyWeb.XmlRpc.IXmlRpcRequest"/></param>
        /// <returns>an associated handler</returns>
        /// <exception cref="LX.EasyWeb.XmlRpc.XmlRpcException">no matched handler is found</exception>
        protected abstract IXmlRpcHandler GetHandler(IXmlRpcRequest request);
    }
}
