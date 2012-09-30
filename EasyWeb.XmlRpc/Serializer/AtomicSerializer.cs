//
// LX.EasyWeb.XmlRpc.Serializer.AtomicSerializer.cs
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
using System.Xml;

namespace LX.EasyWeb.XmlRpc.Serializer
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
                writer.WriteStartElement(tag);
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

    class BooleanSerializer : AtomicSerializer
    {
        protected override String GetTag(IXmlRpcStreamConfig config)
        {
            return XmlRpcSpec.BOOLEAN_TAG;
        }

        protected override String GetString(Object obj)
        {
            return ((Boolean)obj) ? "1" : "0";
        }

        protected override Object DoRead(XmlReader reader)
        {
            return reader.ReadElementContentAsBoolean();
        }
    }

    class Int32Serializer : AtomicSerializer
    {
        protected override String GetTag(IXmlRpcStreamConfig config)
        {
            return config != null && config.UseIntTag ? XmlRpcSpec.INT_TAG : XmlRpcSpec.I4_TAG; ;
        }

        protected override Object DoRead(XmlReader reader)
        {
            return reader.ReadElementContentAsInt();
        }
    }

    class Int64Serializer : AtomicSerializer
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

    class DoubleSerializer : AtomicSerializer
    {
        protected override String GetTag(IXmlRpcStreamConfig config)
        {
            return XmlRpcSpec.DOUBLE_TAG;
        }

        protected override Object DoRead(XmlReader reader)
        {
            return reader.ReadElementContentAsDouble();
        }
    }

    class StringSerializer : AtomicSerializer
    {
        protected override String GetTag(IXmlRpcStreamConfig config)
        {
            return config == null || config.UseStringTag ? XmlRpcSpec.STRING_TAG : null;
        }

        protected override Object DoRead(XmlReader reader)
        {
            return reader.NodeType == XmlNodeType.Text ? reader.ReadContentAsString() : reader.ReadElementContentAsString();
        }
    }
}
