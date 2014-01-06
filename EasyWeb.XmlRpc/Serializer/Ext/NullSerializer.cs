using System;
using System.Xml;

namespace LX.EasyWeb.XmlRpc.Serializer.Ext
{
    class NullSerializer : TypeSerializer
    {
        protected override Object DoRead(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            String result = reader.ReadElementContentAsString();
            if (String.IsNullOrEmpty(result))
                return null;
            else
                throw new XmlRpcException("Unexpected characters in nil element.");
        }

        protected override void DoWrite(XmlWriter writer, Object obj, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            writer.WriteStartElement(XmlRpcSpec.VALUE_TAG);
            writer.WriteStartElement(XmlRpcSpec.EXTENSIONS_PREFIX, XmlRpcSpec.NIL_TAG, XmlRpcSpec.EXTENSIONS_URI);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}
