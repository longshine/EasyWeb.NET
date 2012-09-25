using System;
using System.IO;
using System.IO.Compression;
using LX.EasyWeb.XmlRpc.Parser;
using System.Xml;

namespace LX.EasyWeb.XmlRpc.Server
{
    public abstract class XmlRpcStreamServer : XmlRpcServer
    {
        public void Execute(IXmlRpcStreamRequestConfig config, ServerStream serverStream)
        {
            Object result = null;
            Exception exception = null;
            Stream inputStream;

            try
            {
                inputStream = GetInputStream(config, serverStream);
                IXmlRpcRequest request = GetRequest(config, inputStream);
                result = Execute(request);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Stream outputStream = GetOutputStream(config, serverStream);
            if (exception == null)
                WriteResponse(config, outputStream, result);
            else
                WriteError(config, outputStream, exception);
        }

        private void WriteResponse(IXmlRpcStreamRequestConfig config, Stream outputStream, Object result)
        {
            throw new NotImplementedException();
        }

        private void WriteError(IXmlRpcStreamRequestConfig config, Stream outputStream, Exception result)
        {
            throw new NotImplementedException();
        }

        private IXmlRpcRequest GetRequest(IXmlRpcStreamRequestConfig config, Stream inputStream)
        {
            XmlRpcRequestParser parser = new XmlRpcRequestParser(config, TypeFactory, new XmlTextReader(inputStream));
            return new XmlRpcRequest(parser.MethodName, parser.Parameters);
        }

        protected virtual Stream GetInputStream(IXmlRpcStreamRequestConfig config, ServerStream serverStream)
        {
            Stream inputStream = serverStream.InputStream;
            if (config.EnabledForExtensions && config.GzipCompressing)
                return new GZipStream(inputStream, CompressionMode.Decompress);
            else
                return inputStream;
        }

        protected virtual Stream GetOutputStream(IXmlRpcStreamRequestConfig config, ServerStream serverStream)
        {
            Stream outputStream = serverStream.OutputStream;
            if (config.EnabledForExtensions && config.GzipRequesting)
                return new GZipStream(outputStream, CompressionMode.Compress);
            else
                return outputStream;
        }

        public interface ServerStream
        {
            Stream InputStream { get; }
            Stream OutputStream { get; }
        }
    }
}
