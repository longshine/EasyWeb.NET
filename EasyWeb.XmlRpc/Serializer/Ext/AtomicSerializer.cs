using System;
using System.Xml;

namespace LX.EasyWeb.XmlRpc.Serializer.Ext
{
    abstract class AtomicSerializer : TypeSerializer
    {
        protected override Object DoRead(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            return DoRead(reader);
        }

        protected override void DoWrite(XmlWriter writer, Object obj, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            writer.WriteStartElement(XmlRpcSpec.VALUE_TAG);
            WriteString(writer, GetTag(config), GetString(obj));
            writer.WriteEndElement();
        }

        private void WriteString(XmlWriter writer, String tag, String value)
        {
            if (tag != null)
                writer.WriteStartElement(XmlRpcSpec.EXTENSIONS_PREFIX, tag, XmlRpcSpec.EXTENSIONS_URI);
            writer.WriteString(value);
            if (tag != null)
                writer.WriteEndElement();
        }

        protected virtual String GetString(Object obj)
        {
            return obj.ToString();
        }

        protected abstract String GetTag(IXmlRpcStreamConfig config);
        protected abstract Object DoRead(XmlReader reader);
    }

    class I1Serializer : AtomicSerializer
    {
        protected override String GetTag(IXmlRpcStreamConfig config)
        {
            return XmlRpcSpec.I1_TAG;
        }

        protected override Object DoRead(XmlReader reader)
        {
            String result = reader.ReadElementContentAsString();
            try
            {
                return Byte.Parse(result);
            }
            catch (Exception)
            {
                throw new XmlRpcException("Failed to parse byte value: " + result);
            }
        }
    }

    class I2Serializer : AtomicSerializer
    {
        protected override String GetTag(IXmlRpcStreamConfig config)
        {
            return XmlRpcSpec.I2_TAG;
        }

        protected override Object DoRead(XmlReader reader)
        {
            String result = reader.ReadElementContentAsString();
            try
            {
                return Int16.Parse(result);
            }
            catch (Exception)
            {
                throw new XmlRpcException("Failed to parse byte value: " + result);
            }
        }
    }

    class I8Serializer : AtomicSerializer
    {
        protected override String GetTag(IXmlRpcStreamConfig config)
        {
            return XmlRpcSpec.I8_TAG;
        }

        protected override Object DoRead(XmlReader reader)
        {
            return reader.ReadElementContentAsLong();
        }
    }

    class FloatSerializer : AtomicSerializer
    {
        protected override String GetTag(IXmlRpcStreamConfig config)
        {
            return XmlRpcSpec.FLOAT_TAG;
        }

        protected override Object DoRead(XmlReader reader)
        {
            return reader.ReadElementContentAsFloat();
        }
    }
}
