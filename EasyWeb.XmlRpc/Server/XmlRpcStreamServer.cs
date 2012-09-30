using System;
using System.IO;
using System.IO.Compression;
using System.Xml;
using LX.EasyWeb.XmlRpc.Serializer;

namespace LX.EasyWeb.XmlRpc.Server
{
    public abstract class XmlRpcStreamServer : XmlRpcServer
    {
        private IXmlWriterFactory _xmlWriterFactory = new XmlTextWriterFactory();

        public IXmlWriterFactory XmlWriterFactory
        {
            get { return _xmlWriterFactory; }
            set { _xmlWriterFactory = value; }
        }

        public void Execute(IXmlRpcStreamRequestConfig config, ServerStream serverStream)
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

        private void WriteResponse(IXmlRpcStreamRequestConfig config, Stream outputStream, IXmlRpcResponse response)
        {
            XmlWriter writer = XmlWriterFactory.GetXmlWriter(config, outputStream);
            new XmlRpcResponseSerializer().WriteResponse(writer, response, config, TypeSerializerFactory);
            writer.Close();
        }

        private IXmlRpcRequest GetRequest(IXmlRpcStreamRequestConfig config, Stream inputStream)
        {
            XmlRpcRequestSerializer parser = new XmlRpcRequestSerializer();
            return parser.ReadRequest(new XmlTextReader(inputStream), config, TypeSerializerFactory);
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
