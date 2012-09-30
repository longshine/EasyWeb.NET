//
// LX.EasyWeb.XmlRpc.Serializer.StructSerializer.cs
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
    class StructSerializer : RecursiveTypeSerializer
    {
        protected override Object DoRead(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            IDictionary<String, Object> dic = new Dictionary<String, Object>();

            do
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    CheckTag(reader, XmlRpcSpec.MEMBER_TAG);
                    do
                    {
                        reader.Read();
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            CheckTag(reader, XmlRpcSpec.MEMBER_NAME_TAG);

                            String name = reader.ReadElementContentAsString();
                            if (name == null)
                                throw new XmlException("Name is expected for struct members.");

                            while (reader.NodeType != XmlNodeType.Element && reader.Read())
                                ;

                            if (dic.ContainsKey(name))
                                throw new XmlException("Duplicate struct member name: " + name);

                            Object value = ReadValue(reader, config, typeSerializerFactory);
                            dic.Add(name, value);
                        }
                    } while (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.MEMBER_TAG.Equals(reader.LocalName));
                }
            } while (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.STRUCT_TAG.Equals(reader.LocalName));

            return dic;
        }

        protected override void DoWrite(XmlWriter writer, Object obj, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory, IList nestedObjs)
        {
            writer.WriteStartElement(XmlRpcSpec.VALUE_TAG);
            writer.WriteStartElement(XmlRpcSpec.STRUCT_TAG);

            foreach (DictionaryEntry entry in (IDictionary)obj)
            {
                writer.WriteStartElement(XmlRpcSpec.MEMBER_TAG);

                writer.WriteStartElement(XmlRpcSpec.MEMBER_NAME_TAG);
                if (config.EnabledForExtensions && !(entry.Key is String))
                    WriteValue(writer, entry.Key, config, typeSerializerFactory, nestedObjs);
                else
                    writer.WriteString(entry.Key.ToString());
                writer.WriteEndElement();

                WriteValue(writer, entry.Value, config, typeSerializerFactory, nestedObjs);

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}
