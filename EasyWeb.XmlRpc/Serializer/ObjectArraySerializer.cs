//
// LX.EasyWeb.XmlRpc.Serializer.ObjectArraySerializer.cs
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
using System.Collections.Generic;
using System.Xml;

namespace LX.EasyWeb.XmlRpc.Serializer
{
    class ObjectArraySerializer : RecursiveTypeSerializer
    {
        protected override Object DoRead(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            List<Object> list = new List<Object>();

            do
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    CheckTag(reader, XmlRpcSpec.DATA_TAG);
                    do
                    {
                        reader.Read();
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            list.Add(ReadValue(reader, config, typeSerializerFactory));
                        }
                    } while (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.DATA_TAG.Equals(reader.LocalName));
                }
            } while (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.ARRAY_TAG.Equals(reader.LocalName));

            return list.ToArray();
        }

        protected override void DoWrite(XmlWriter writer, Object obj, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory, IList nestedObjs)
        {
            writer.WriteStartElement(XmlRpcSpec.VALUE_TAG);
            writer.WriteStartElement(XmlRpcSpec.ARRAY_TAG);
            writer.WriteStartElement(XmlRpcSpec.DATA_TAG);

            foreach (var value in (IEnumerable)obj)
            {
                WriteValue(writer, value, config, typeSerializerFactory, nestedObjs);
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}
