using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

namespace LX.EasyWeb.XmlRpc.Server
{
    public abstract class XmlRpcHttpServer : XmlRpcStreamServer
    {
        protected void HandleHttpRequest(HttpRequest request, HttpResponse response)
        {
            if (request.HttpMethod == "GET")
                HandleGET(request, response);
            else if (request.HttpMethod == "POST")
                HandlePOST(request, response);
            else
                HandleUnsupportedMethod(request, response);
        }

        protected override Stream GetOutputStream(IXmlRpcStreamRequestConfig config, ServerStream serverStream)
        {
            if (config.EnabledForExtensions && config.GzipRequesting)
                SetResponseHeader(serverStream, HttpHelper.ContentEncodingHeader, "gzip");
            return base.GetOutputStream(config, serverStream);
        }

        private void HandlePOST(HttpRequest request, HttpResponse response)
        {
            response.ContentType = "text/xml";
            IXmlRpcStreamRequestConfig config = GetConfig(request);
            ServerStream stream = new HttpStream(request, response);
            Execute(config, stream);
        }

        private void HandleGET(HttpRequest request, HttpResponse response)
        {
#if DEBUG
            foreach (var method in GetListMethods())
            {
                response.Write(method);
            }
#endif
        }

        private void HandleUnsupportedMethod(HttpRequest request, HttpResponse response)
        {
            // RFC 2068 error 405: "The method specified in the Request-Line   
            // is not allowed for the resource identified by the Request-URI. 
            // The response MUST include an Allow header containing a list 
            // of valid methods for the requested resource."
            // TODO add Allow header
            response.StatusCode = 405;
            response.StatusDescription = "Unsupported HTTP verb";
        }

        private IXmlRpcStreamRequestConfig GetConfig(HttpRequest request)
        {
            XmlRpcHttpRequestConfig rc = new XmlRpcHttpRequestConfig();
            IXmlRpcHttpServerConfig serverConfig = (IXmlRpcHttpServerConfig)Config;
            rc.BasicEncoding = serverConfig.BasicEncoding;
            rc.ContentLengthOptional = serverConfig.ContentLengthOptional && (request.Headers[HttpHelper.ContentLengthHeader] == null);
            rc.EnabledForExtensions = serverConfig.EnabledForExtensions;
            rc.GzipCompressing = HttpHelper.HasGzipEncoding(request.Headers[HttpHelper.ContentEncodingHeader]);
            rc.GzipRequesting = HttpHelper.HasGzipEncoding(request.Headers[HttpHelper.AcceptEncodingHeader]);
            rc.Encoding = request.ContentEncoding.WebName;
            rc.EnabledForExceptions = serverConfig.EnabledForExceptions;
            HttpHelper.ParseAuthorization(rc, request.Headers[HttpHelper.AuthorizationHeader]);
            return rc;
        }

        private void SetResponseHeader(ServerStream serverStream, String header, String value)
        {
            ((HttpStream)serverStream).Response.AppendHeader(header, value);
        }

        class HttpStream : ServerStream
        {
            public HttpStream(HttpRequest request, HttpResponse response)
            {
                Request = request;
                Response = response;
            }

            public HttpRequest Request { get; private set; }

            public HttpResponse Response { get; private set; }

            public Stream InputStream
            {
                get { return Request.InputStream; }
            }

            public Stream OutputStream
            {
                get { return Response.OutputStream; }
            }
        }
    }
}
