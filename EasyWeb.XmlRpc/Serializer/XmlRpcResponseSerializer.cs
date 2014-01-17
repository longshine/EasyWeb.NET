//
// LX.EasyWeb.XmlRpc.Serializer.XmlRpcResponseSerializer.cs
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
using System.IO;
using System.Xml;

namespace LX.EasyWeb.XmlRpc.Serializer
{
    /// <summary>
    /// Provides methods to serialize and deserialize XML-RPC responses.
    /// </summary>
    public class XmlRpcResponseSerializer : ITypeSerializer
    {
        private IXmlWriterFactory _xmlWriterFactory = new XmlTextWriterFactory();

        /// <summary>
        /// Gets or sets the factory that creates <see cref="System.Xml.XmlWriter"/>.
        /// </summary>
        public IXmlWriterFactory XmlWriterFactory
        {
            get { return _xmlWriterFactory; }
            set { _xmlWriterFactory = value; }
        }

        /// <summary>
        /// Serializes an XML-RPC response to a <see cref="System.Xml.XmlWriter"/>.
        /// </summary>
        /// <param name="writer">the <see cref="System.Xml.XmlWriter"/> to write</param>
        /// <param name="response">the <see cref="LX.EasyWeb.XmlRpc.IXmlRpcResponse"/> to serialize</param>
        /// <param name="config">the context configuration</param>
        /// <param name="typeSerializerFactory">the <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializerFactory"/> to get type serializers</param>
        /// <exception cref="System.Xml.XmlException">failed writing the response XML</exception>
        public void WriteResponse(XmlWriter writer, IXmlRpcResponse response, IXmlRpcStreamRequestConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement(XmlRpcSpec.METHOD_RESPONSE_TAG);

            if (response.Fault == null)
                RecursiveTypeSerializer.WriteParams(writer, config, typeSerializerFactory, response.Result);
            else
                WriteFaultResponse(writer, response.Fault, config, typeSerializerFactory);

            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        /// <summary>
        /// Serializes an XML-RPC response to a response stream.
        /// </summary>
        /// <param name="responseStream">the <see cref="System.IO.Stream"/> to write</param>
        /// <param name="response">the <see cref="LX.EasyWeb.XmlRpc.IXmlRpcResponse"/> to serialize</param>
        /// <param name="config">the context configuration</param>
        /// <param name="typeSerializerFactory">the <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializerFactory"/> to get type serializers</param>
        /// <exception cref="System.Xml.XmlException">failed writing the response XML</exception>
        public void WriteResponse(Stream responseStream, IXmlRpcResponse response, IXmlRpcStreamRequestConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            XmlWriter writer = _xmlWriterFactory.GetXmlWriter(config, responseStream);
            WriteResponse(writer, response, config, typeSerializerFactory);
            writer.Flush();
        }

        /// <summary>
        /// Deserializes an XML-RPC response from a <see cref="System.Xml.XmlReader"/>.
        /// </summary>
        /// <param name="reader">the <see cref="System.Xml.XmlReader"/> to read</param>
        /// <param name="config">the context configuration</param>
        /// <param name="typeSerializerFactory">the <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializerFactory"/> to get type serializers</param>
        /// <returns>a <see cref="LX.EasyWeb.XmlRpc.IXmlRpcResponse"/> read from the reader</returns>
        /// <exception cref="System.Xml.XmlException">failed parsing the response XML</exception>
        public IXmlRpcResponse ReadResponse(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            IXmlRpcResponse response = null;
            do
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (!String.IsNullOrEmpty(reader.NamespaceURI) || !XmlRpcSpec.METHOD_RESPONSE_TAG.Equals(reader.LocalName))
                        throw new XmlException("Expected root element methodResponse, got " + new XmlQualifiedName(reader.LocalName, reader.NamespaceURI));

                    RecursiveTypeSerializer.ReadToElement(reader);
                    if (String.IsNullOrEmpty(reader.NamespaceURI) && XmlRpcSpec.FAULT_TAG.Equals(reader.LocalName))
                        response = ReadFaultResponse(reader, config, typeSerializerFactory);
                    else if (String.IsNullOrEmpty(reader.NamespaceURI) && XmlRpcSpec.PARAMS_TAG.Equals(reader.LocalName))
                        response = ReadParamReponse(reader, config, typeSerializerFactory);
                }
            } while (!reader.EOF && (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.METHOD_RESPONSE_TAG.Equals(reader.LocalName)));

            if (response == null)
                throw new XmlRpcException("Invalid XML-RPC response.");

            return response;
        }

        private static IXmlRpcResponse ReadParamReponse(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            IList list = RecursiveTypeSerializer.ReadParams(reader, config, typeSerializerFactory);
            XmlRpcResponse response = new XmlRpcResponse(list.Count > 0 ? list[0] : null);
            return response;
        }

        private static IXmlRpcResponse ReadFaultResponse(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            IDictionary dict = null;
            do
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    dict = (IDictionary)RecursiveTypeSerializer.ReadValue(reader, config, typeSerializerFactory);
                }
            } while (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.FAULT_TAG.Equals(reader.LocalName));

            if (dict != null)
            {
                if (!dict.Contains(XmlRpcSpec.FAULT_CODE_TAG))
                    throw new XmlRpcException("Invalid XML-RPC response: missing fault code");
                if (!dict.Contains(XmlRpcSpec.FAULT_STRING_TAG))
                    throw new XmlRpcException("Invalid XML-RPC response: missing fault string");

                try
                {
                    XmlRpcFault fault = new XmlRpcFault((Int32)dict[XmlRpcSpec.FAULT_CODE_TAG], (String)dict[XmlRpcSpec.FAULT_STRING_TAG]);
                    XmlRpcResponse response = new XmlRpcResponse(null);
                    response.Fault = fault;
                    return response;
                }
                catch (Exception ex)
                {
                    throw new XmlRpcException("Invalid XML-RPC response: " + ex.Message);
                }
            }

            throw new XmlRpcException("Invalid XML-RPC response.");
        }

        private static void WriteFaultResponse(XmlWriter writer, IXmlRpcFault fault, IXmlRpcStreamRequestConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            writer.WriteStartElement(XmlRpcSpec.FAULT_TAG);

            ArrayList nestvedObjs = new ArrayList();
            if (config != null && config.EnabledForExceptions)
            {
                Hashtable ht = new Hashtable(3);
                XmlRpcFault xpf = fault as XmlRpcFault;
                ht.Add(XmlRpcSpec.FAULT_CODE_TAG, fault.FaultCode);
                ht.Add(XmlRpcSpec.FAULT_STRING_TAG, fault.FaultString);
                ht.Add("faultCause", fault is XmlRpcFault ? ((XmlRpcFault)fault).Exception : null);
                RecursiveTypeSerializer.WriteValue(writer, ht, config, typeSerializerFactory, nestvedObjs);
            }
            else
            {
                RecursiveTypeSerializer.WriteValue(writer, fault, config, typeSerializerFactory, nestvedObjs);
            }

            writer.WriteEndElement();
        }

        Object ITypeSerializer.Read(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            return ReadResponse(reader, config, typeSerializerFactory);
        }

        void ITypeSerializer.Write(XmlWriter writer, Object obj, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            WriteResponse(writer, (IXmlRpcResponse)obj, config as IXmlRpcStreamRequestConfig, typeSerializerFactory);
        }
    }
}
