//
// LX.EasyWeb.XmlRpc.Serializer.RecursiveTypeSerializer.cs
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
    abstract class RecursiveTypeSerializer : TypeSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="System.Xml.XmlException"></exception>
        public static Object ReadValue(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeFactory)
        {
            CheckTag(reader, XmlRpcSpec.VALUE_TAG);

            Object value = null;

            do
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.IsEmptyElement)
                        continue;
                    ITypeSerializer parser = typeFactory.GetSerializer(config, reader.NamespaceURI, reader.LocalName);
                    if (parser == null)
                        throw new XmlException("Unknown type: " + new XmlQualifiedName(reader.LocalName, reader.NamespaceURI));
                    else
                        value = parser.Read(reader, config, typeFactory);
                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    ITypeSerializer parser = typeFactory.GetSerializer(config, reader.NamespaceURI, String.Empty);
                    if (parser != null)
                        value = parser.Read(reader, config, typeFactory);
                }
            } while (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.VALUE_TAG.Equals(reader.LocalName));

            return value;
        }

        public static IList ReadParams(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            if (reader.EOF)
                return null;
            CheckTag(reader, XmlRpcSpec.PARAMS_TAG);
            IList args = new ArrayList();
            do
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    args.Add(ReadParam(reader, config, typeSerializerFactory));
                }
            } while (!reader.EOF && (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.PARAMS_TAG.Equals(reader.LocalName)));
            return args;
        }

        public static void WriteValue(XmlWriter writer, Object value, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory, IList nestedObjs)
        {
            if (nestedObjs.Contains(value))
                return;
            nestedObjs.Add(value);
            ITypeSerializer serializer = typeSerializerFactory.GetSerializer(config, value);
            if (serializer == null)
                throw new XmlRpcException("Unsupported type: " + value.GetType().Name);
            if (serializer is RecursiveTypeSerializer)
                ((RecursiveTypeSerializer)serializer).DoWrite(writer, value, config, typeSerializerFactory, nestedObjs);
            else
                serializer.Write(writer, value, config, typeSerializerFactory);
            nestedObjs.RemoveAt(nestedObjs.Count - 1);
        }

        public static void WriteParams(XmlWriter writer, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory, params Object[] parameters)
        {
            writer.WriteStartElement(XmlRpcSpec.PARAMS_TAG);

            ArrayList nestedObjs = new ArrayList();
            foreach (var item in parameters)
            {
                writer.WriteStartElement(XmlRpcSpec.PARAM_TAG);
                WriteValue(writer, item, config, typeSerializerFactory, nestedObjs);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        public static void ReadToElement(XmlReader reader)
        {
            while (reader.Read() && reader.NodeType != XmlNodeType.Element)
                ;
        }

        public static void CheckTag(XmlReader reader, String expectedTag)
        {
            if (!String.IsNullOrEmpty(reader.NamespaceURI) || !expectedTag.Equals(reader.LocalName))
                throw new XmlException("Expected " + expectedTag + " element, got " + new XmlQualifiedName(reader.LocalName, reader.NamespaceURI));
        }
        
        protected override void DoWrite(XmlWriter writer, Object obj, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
#if DEBUG
            throw new NotSupportedException("Call DoWrite(writer, value, config, typeSerializerFactory, nestedObjs) instead");
#else
            WriteValue(writer, obj, config, typeSerializerFactory, new ArrayList());
#endif
        }

        protected abstract void DoWrite(XmlWriter writer, Object value, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory, IList nestedObjs);

        private static Object ReadParam(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            CheckTag(reader, XmlRpcSpec.PARAM_TAG);
            Object param = null;
            Int32 count = 0;
            do
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element && count++ == 0)
                {
                    param = ReadValue(reader, config, typeSerializerFactory);
                }
            } while (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.PARAM_TAG.Equals(reader.LocalName));
            if (count == 0)
                throw new XmlException("No value element in the param element.");
            else if (count > 1)
                throw new XmlException("More than one value element in the param element.");
            return param;
        }
    }
}
