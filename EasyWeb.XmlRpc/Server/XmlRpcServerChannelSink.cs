//
// LX.EasyWeb.XmlRpc.Server.XmlRpcServerChannelSink.cs
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
using System.Collections;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using LX.EasyWeb.XmlRpc.Serializer;
using System.Collections.Generic;

namespace LX.EasyWeb.XmlRpc.Server
{
    public class XmlRpcServerChannelSink : IServerChannelSink
    {
        private XmlRpcServerConfig _config = new XmlRpcServerConfig();
        private TypeReflectiveHandlerMapping _mapping = new TypeReflectiveHandlerMapping();

        public XmlRpcServerChannelSink(IServerChannelSink next)
        {
            NextChannelSink = next;
        }

        public IXmlRpcServerConfig Config { get; set; }

        public IServerChannelSink NextChannelSink { get; private set; }

        public IDictionary Properties
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or set the <see cref="LX.EasyWeb.XmlRpc.ITypeSerializerFactory"/>.
        /// </summary>
        public ITypeSerializerFactory TypeSerializerFactory { get; set; }

        public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, Object state, IMessage msg, ITransportHeaders headers, Stream stream)
        {
            throw new NotImplementedException();
        }

        public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, Object state, IMessage msg, ITransportHeaders headers)
        {
            throw new NotImplementedException();
        }

        public ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack,
            IMessage requestMsg, ITransportHeaders requestHeaders, Stream requestStream,
            out IMessage responseMsg, out ITransportHeaders responseHeaders, out Stream responseStream)
        {
            if (requestHeaders["SOAPAction"] != null)
            {
                // pass to next sink if this is a SOAP request with SOAPAction header.
                return NextChannelSink.ProcessMessage(sinkStack,
                    requestMsg, requestHeaders, requestStream,
                    out responseMsg, out responseHeaders, out responseStream);
            }

            XmlRpcHttpRequestConfig requestConfig = GetConfig(requestHeaders);

            try
            {
                MethodCall call = DeserializeRequest(requestHeaders, requestStream, requestConfig);
                sinkStack.Push(this, call);
                // forward to next sink in chain - pass request stream as null to 
                // indicate that we have deserialized the request
                NextChannelSink.ProcessMessage(sinkStack, call, requestHeaders, null,
                  out responseMsg, out responseHeaders, out responseStream);
                SerializeResponse(responseMsg, ref responseHeaders, ref responseStream, requestConfig);
            }
            catch (Exception ex)
            {
                responseMsg = new ReturnMessage(ex, (IMethodCallMessage)requestMsg);
                responseStream = new MemoryStream();
                XmlRpcResponseSerializer serializer = new XmlRpcResponseSerializer();
                XmlRpcResponse faultResponse = new XmlRpcResponse();
                faultResponse.Fault = new XmlRpcFault(0, ex.Message);
                serializer.WriteResponse(responseStream, faultResponse, requestConfig, TypeSerializerFactory);
                responseHeaders = new TransportHeaders();
            }

            return ServerProcessing.Complete;
        }

        private MethodCall DeserializeRequest(ITransportHeaders requestHeaders, Stream requestStream, IXmlRpcStreamRequestConfig config)
        {
            String requestUri = GetRequestUri(requestHeaders);
            Type svcType = GetServerTypeForUri(requestUri);
            IXmlRpcRequest request = GetRequest(config, requestStream);
            IXmlRpcHandler handler = GetHandler(request);
            Header[] headers = GetChannelHeaders(requestUri, request, handler, svcType);
            MethodCall call = new MethodCall(headers);
            call.ResolveMethod();
            return call;
        }

        private void SerializeResponse(IMessage responseMsg, ref ITransportHeaders responseHeaders, ref Stream responseStream, IXmlRpcStreamRequestConfig config)
        {
            XmlRpcResponseSerializer serializer = new XmlRpcResponseSerializer();
            responseStream = new MemoryStream();
            responseHeaders = new TransportHeaders();

            ReturnMessage retMsg = (ReturnMessage)responseMsg;
            XmlRpcResponse response;
            if (retMsg.Exception == null)
            {
                response = new XmlRpcResponse(retMsg.ReturnValue);
            }
            else if (retMsg.Exception is XmlRpcException)
            {
                response = new XmlRpcResponse();
                response.Fault = (retMsg.Exception as XmlRpcException).Fault;
            }
            else
            {
                response = new XmlRpcResponse();
                response.Fault = new XmlRpcFault(1, retMsg.Exception.Message);
            }
            serializer.WriteResponse(responseStream, response, config, TypeSerializerFactory);
            responseHeaders["Content-Type"] = "text/xml; charset=\"utf-8\"";
        }

        private Type GetServerTypeForUri(String uri)
        {
            Type type = RemotingServices.GetServerTypeForUri(uri);
            if (type == null)
                throw new NotSupportedException("No service type registered for uri " + uri);
            return type;
        }

        private XmlRpcHttpRequestConfig GetConfig(ITransportHeaders requestHeaders)
        {
            XmlRpcHttpRequestConfig rc = new XmlRpcHttpRequestConfig();
            IXmlRpcHttpServerConfig serverConfig = (IXmlRpcHttpServerConfig)Config;
            rc.BasicEncoding = serverConfig.BasicEncoding;
            rc.ContentLengthOptional = serverConfig.ContentLengthOptional && (HttpHelper.GetContentLength(requestHeaders) == null);
            rc.EnabledForExtensions = serverConfig.EnabledForExtensions;
            rc.GzipCompressing = HttpHelper.IsUsingGzipEncoding(requestHeaders);
            rc.GzipRequesting = HttpHelper.IsAcceptingGzipEncoding(requestHeaders);
            // TODO get request encoding
            //rc.Encoding
            rc.EnabledForExceptions = serverConfig.EnabledForExceptions;
            HttpHelper.ParseAuthorization(rc, HttpHelper.GetAuthorization(requestHeaders));
            return rc;
        }

        private IXmlRpcHandler GetHandler(IXmlRpcRequest request)
        {
            return _mapping.GetHandler(request.MethodName);
        }

        private  IXmlRpcRequest GetRequest(IXmlRpcStreamRequestConfig config, Stream inputStream)
        {
            XmlRpcRequestSerializer parser = new XmlRpcRequestSerializer();
            return parser.ReadRequest(inputStream, config, TypeSerializerFactory);
        }

        private static Header[] GetChannelHeaders(String requestUri, IXmlRpcRequest request, IXmlRpcHandler handler, Type type)
        {
            List<Header> headers = new List<Header>();
            headers.Add(new Header("__Uri", requestUri));
            headers.Add(new Header("__TypeName", type.AssemblyQualifiedName));
            headers.Add(new Header("__MethodName", handler.GetMethod(request).Name));
            headers.Add(new Header("__Args", request.Parameters));
            return headers.ToArray();
        }

        private static String GetRequestUri(ITransportHeaders requestHeaders)
        {
            return (String)requestHeaders["__RequestUri"];
        }
    }
}
