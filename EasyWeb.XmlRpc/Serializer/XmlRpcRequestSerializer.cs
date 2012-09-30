//
// LX.EasyWeb.XmlRpc.Serializer.XmlRpcRequestSerializer.cs
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
using System.Xml;

namespace LX.EasyWeb.XmlRpc.Serializer
{
    public class XmlRpcRequestSerializer : ITypeSerializer
    {
        public IXmlRpcRequest ReadRequest(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            String methodName = null;
            IList args = null;
            do
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (!String.IsNullOrEmpty(reader.NamespaceURI) || !XmlRpcSpec.METHOD_CALL_TAG.Equals(reader.LocalName))
                        throw new XmlException("Expected root element methodCall, got " + new XmlQualifiedName(reader.LocalName, reader.NamespaceURI));

                    RecursiveTypeSerializer.ReadToElement(reader);
                    methodName = ReadMethodName(reader);
                    RecursiveTypeSerializer.ReadToElement(reader);
                    args = RecursiveTypeSerializer.ReadParams(reader, config, typeSerializerFactory);
                }
            } while (!reader.EOF && (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.METHOD_CALL_TAG.Equals(reader.LocalName)));
            return new XmlRpcRequest(methodName, args == null ? null : ToArray(args));
        }

        public void WriteRequest(XmlWriter writer, IXmlRpcRequest request, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement(XmlRpcSpec.METHOD_CALL_TAG);

            writer.WriteElementString(XmlRpcSpec.METHOD_NAME_TAG, request.MethodName);
            RecursiveTypeSerializer.WriteParams(writer, config, typeSerializerFactory, request.Parameters);

            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        private Object[] ToArray(IList list)
        {
            Object[] array = new Object[list.Count];
            for (Int32 i = 0; i < array.Length; i++)
            {
                array[i] = list[i];
            }
            return array;
        }

        private static String ReadMethodName(XmlReader reader)
        {
            RecursiveTypeSerializer.CheckTag(reader, XmlRpcSpec.METHOD_NAME_TAG);
            return reader.ReadElementString();
        }

        object ITypeSerializer.Read(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            return ReadRequest(reader, config, typeSerializerFactory);
        }

        void ITypeSerializer.Write(XmlWriter writer, object obj, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            WriteRequest(writer, (IXmlRpcRequest)obj, config, typeSerializerFactory);
        }
    }
}
