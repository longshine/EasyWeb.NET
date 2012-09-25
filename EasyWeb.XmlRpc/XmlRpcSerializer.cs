using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using LX.EasyWeb.XmlRpc.Parser;

namespace LX.EasyWeb.XmlRpc
{
    class XmlRpcSerializer
    {
        public XmlRpcSerializer()
        {
            Indentation = 2;
            UseEmptyParamsTag = true;
            UseIndentation = true;
            UseStringTag = true;
        }

        public Int32 Indentation { get; set; }
        public Boolean UseEmptyParamsTag { get; set; }
        public Boolean UseIndentation { get; set; }
        public Boolean UseIntTag { get; set; }
        public Boolean UseStringTag { get; set; }
        public Encoding XmlEncoding { get; set; }

        public static IXmlRpcRequest DeserializeRequest(Stream stream, IXmlRpcStreamConfig config, ITypeFactory typeFactory, Type type)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            XmlTextReader reader = new XmlTextReader(stream);
            XmlRpcRequestParser parser = new XmlRpcRequestParser(config, typeFactory, reader);

            return null;
        }
    }
}
