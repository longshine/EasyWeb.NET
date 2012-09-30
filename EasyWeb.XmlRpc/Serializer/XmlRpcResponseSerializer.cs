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
using System.Xml;

namespace LX.EasyWeb.XmlRpc.Serializer
{
    public class XmlRpcResponseSerializer : ITypeSerializer
    {
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
