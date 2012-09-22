//
// LX.EasyWeb.XmlRpc.Parser.ITypeParser.cs
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
    /// Provides methods to parse value from a <see cref="System.Xml.XmlReader"/>.
    /// </summary>
    public interface ITypeParser
    {
        /// <summary>
        /// Reads a typed value.
        /// </summary>
        /// <param name="reader">the <see cref="System.Xml.XmlReader"/> to read</param>
        /// <param name="config">the <see cref="LX.EasyWeb.XmlRpc.IXmlRpcStreamConfig"/></param>
        /// <param name="typeFactory">the <see cref="LX.EasyWeb.XmlRpc.ITypeFactory"/> to create parsers</param>
        /// <returns>an parsed value</returns>
        /// <exception cref="System.Xml.XmlException">throws if error occurs during parsing</exception>
        Object Read(XmlReader reader, IXmlRpcStreamConfig config, ITypeFactory typeFactory);
    }

    static class ThrowHelper
    {
        public static void ThrowUnexpectedTag(String expectedTag, XmlReader reader)
        {
            throw new XmlException("Expected " + expectedTag + " element, got " + new XmlQualifiedName(reader.LocalName, reader.NamespaceURI));
        }

        public static void ThrowUnknowType(XmlReader reader)
        {
            throw new XmlException("Unknown type: " + new XmlQualifiedName(reader.LocalName, reader.NamespaceURI));
        }
    }
}
