using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting;

namespace LX.EasyWeb.XmlRpc
{
    public class XmlRpcServerChannelSink : IServerChannelSink
    {
        private XmlRpcServerConfig config = new XmlRpcServerConfig();

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

            try
            {
                MethodCall call = DeserializeRequest(requestHeaders, requestStream);
                sinkStack.Push(this, call);
                // forward to next sink in chain - pass request stream as null to 
                // indicate that we have deserialized the request
                NextChannelSink.ProcessMessage(sinkStack, call, requestHeaders, null,
                  out responseMsg, out responseHeaders, out responseStream);
                SerializeResponse(responseMsg, ref responseHeaders, ref responseStream);
            }
            catch (Exception)
            {

                throw;
            }

            return ServerProcessing.Complete;
        }

        private MethodCall DeserializeRequest(ITransportHeaders requestHeaders, Stream requestStream)
        {
            String requestUri = HttpHelper.GetRequestUri(requestHeaders);
            Type svcType = GetServerTypeForUri(requestUri);
            XmlRpcHttpRequestConfig config = GetConfig(requestHeaders);
            XmlRpcSerializer serializer = new XmlRpcSerializer();

            return null;
        }

        private void SerializeResponse(IMessage responseMsg, ref ITransportHeaders responseHeaders, ref Stream responseStream)
        {
            throw new NotImplementedException();
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
    }
}
