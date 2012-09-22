//
// LX.EasyWeb.XmlRpc.Parser.RecursiveTypeParser.cs
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

namespace LX.EasyWeb.XmlRpc.Parser
{
    /// <summary>
    /// Base class of recursive parsers.
    /// </summary>
    abstract class RecursiveTypeParser : ITypeParser
    {
        public static Object ReadValue(XmlReader reader, IXmlRpcStreamConfig config, ITypeFactory typeFactory)
        {
            CheckTag(reader, XmlRpcSpec.VALUE_TAG);

            Object value = null;

            do
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    ITypeParser parser = typeFactory.GetParser(config, reader.NamespaceURI, reader.LocalName);
                    if (parser == null)
                        ThrowHelper.ThrowUnknowType(reader);
                    else
                        value = parser.Read(reader, config, typeFactory);
                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    ITypeParser parser = typeFactory.GetParser(config, reader.NamespaceURI, String.Empty);
                    if (parser != null)
                        value = parser.Read(reader, config, typeFactory);
                }
            } while (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.VALUE_TAG.Equals(reader.LocalName));

            return value;
        }

        public static void CheckTag(XmlReader reader, String tag)
        {
            if (!String.IsNullOrEmpty(reader.NamespaceURI) || !tag.Equals(reader.LocalName))
                ThrowHelper.ThrowUnexpectedTag(tag, reader);
        }

        public Object Read(XmlReader reader, IXmlRpcStreamConfig config, ITypeFactory typeFactory)
        {
            return DoReadResult(reader, config, typeFactory);
        }

        protected abstract Object DoReadResult(XmlReader reader, IXmlRpcStreamConfig config, ITypeFactory typeFactory);
    }
}
