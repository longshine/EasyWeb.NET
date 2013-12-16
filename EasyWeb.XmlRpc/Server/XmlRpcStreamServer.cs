//
// LX.EasyWeb.XmlRpc.Server.XmlRpcStreamServer.cs
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
using System.IO;
using System.IO.Compression;
using System.Xml;
using LX.EasyWeb.XmlRpc.Serializer;

namespace LX.EasyWeb.XmlRpc.Server
{
    /// <summary>
    /// Subclass of <see cref="LX.EasyWeb.XmlRpc.Server.XmlRpcServer"/> with support for reading requests from and writing responses to streams.
    /// </summary>
    public abstract class XmlRpcStreamServer : XmlRpcServer, IXmlRpcStreamRequestProcessor
    {
        private IXmlWriterFactory _xmlWriterFactory;

        /// <summary>
        /// Gets or sets the factory that creates <see cref="System.Xml.XmlWriter"/>.
        /// </summary>
        public IXmlWriterFactory XmlWriterFactory
        {
            get { return _xmlWriterFactory; }
            set { _xmlWriterFactory = value; }
        }

        /// <summary>
        /// Executes requests from streams.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="serverStream">the <see cref="LX.EasyWeb.XmlRpc.Server.XmlRpcStreamServer+ServerStream"/> to process</param>
        public void Execute(IXmlRpcStreamRequestConfig config, IServerStream serverStream)
        {
            Stream inputStream;
            XmlRpcResponse response = new XmlRpcResponse();

            try
            {
                inputStream = GetInputStream(config, serverStream);
                IXmlRpcRequest request = GetRequest(config, inputStream);
                response.Result = Execute(request);
            }
            catch (XmlRpcException ex)
            {
                response.Fault = ex.Fault;
            }
            catch (Exception ex)
            {
                response.Fault = new XmlRpcFault(0, ex.Message, ex);
            }

            Stream outputStream = GetOutputStream(config, serverStream);
            WriteResponse(config, outputStream, response);
        }

        /// <summary>
        /// Gets input stream.
        /// </summary>
        protected virtual Stream GetInputStream(IXmlRpcStreamRequestConfig config, IServerStream serverStream)
        {
            Stream inputStream = serverStream.InputStream;
            if (config.EnabledForExtensions && config.GzipCompressing)
                return new GZipStream(inputStream, CompressionMode.Decompress);
            else
                return inputStream;
        }

        /// <summary>
        /// Gets output stream.
        /// </summary>
        protected virtual Stream GetOutputStream(IXmlRpcStreamRequestConfig config, IServerStream serverStream)
        {
            Stream outputStream = serverStream.OutputStream;
            if (config.EnabledForExtensions && config.GzipRequesting)
                return new GZipStream(outputStream, CompressionMode.Compress);
            else
                return outputStream;
        }

        private void WriteResponse(IXmlRpcStreamRequestConfig config, Stream outputStream, IXmlRpcResponse response)
        {
            XmlRpcResponseSerializer serializer = new XmlRpcResponseSerializer();
            if (_xmlWriterFactory != null)
                serializer.XmlWriterFactory = _xmlWriterFactory;
            serializer.WriteResponse(outputStream, response, config, TypeSerializerFactory);
        }

        private IXmlRpcRequest GetRequest(IXmlRpcStreamRequestConfig config, Stream inputStream)
        {
            XmlRpcRequestSerializer parser = new XmlRpcRequestSerializer();
            return parser.ReadRequest(new XmlTextReader(inputStream), config, TypeSerializerFactory);
        }
    }
}
