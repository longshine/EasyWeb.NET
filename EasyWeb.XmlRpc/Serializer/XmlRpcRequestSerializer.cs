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
    /// <summary>
    /// Provides methods to serialize and deserialize XML-RPC requests.
    /// </summary>
    public class XmlRpcRequestSerializer : ITypeSerializer
    {
        /// <summary>
        /// Deserializes an XML-RPC request from a <see cref="System.Xml.XmlReader"/>.
        /// </summary>
        /// <param name="reader">the <see cref="System.Xml.XmlReader"/> to read</param>
        /// <param name="config">the context configuration</param>
        /// <param name="typeSerializerFactory">the <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializerFactory"/> to get type serializers</param>
        /// <returns>a <see cref="LX.EasyWeb.XmlRpc.IXmlRpcRequest"/> read from the reader</returns>
        /// <exception cref="System.Xml.XmlException">failed parsing the request XML</exception>
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

        /// <summary>
        /// Serializes an XML-RPC request to a <see cref="System.Xml.XmlWriter"/>.
        /// </summary>
        /// <param name="writer">the <see cref="System.Xml.XmlWriter"/> to write</param>
        /// <param name="request">the <see cref="LX.EasyWeb.XmlRpc.IXmlRpcRequest"/> to serialize</param>
        /// <param name="config">the context configuration</param>
        /// <param name="typeSerializerFactory">the <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializerFactory"/> to get type serializers</param>
        /// <exception cref="System.Xml.XmlException">failed writing the request XML</exception>
        public void WriteRequest(XmlWriter writer, IXmlRpcRequest request, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement(XmlRpcSpec.METHOD_CALL_TAG);

            writer.WriteElementString(XmlRpcSpec.METHOD_NAME_TAG, request.MethodName);
            RecursiveTypeSerializer.WriteParams(writer, config, typeSerializerFactory, request.Parameters);

            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        private static Object[] ToArray(IList list)
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

        Object ITypeSerializer.Read(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            return ReadRequest(reader, config, typeSerializerFactory);
        }

        void ITypeSerializer.Write(XmlWriter writer, Object obj, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            WriteRequest(writer, (IXmlRpcRequest)obj, config, typeSerializerFactory);
        }
    }
}
