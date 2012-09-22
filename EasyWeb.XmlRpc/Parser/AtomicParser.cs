//
// LX.EasyWeb.XmlRpc.Parser.AtomicParser.cs
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

namespace LX.EasyWeb.XmlRpc.Parser
{
    /// <summary>
    /// Base class of parsers for parsing an atomic value.
    /// </summary>
    public abstract class AtomicParser : ITypeParser
    {
        /// <summary>
        /// Reads a typed value.
        /// </summary>
        /// <param name="reader">the <see cref="System.Xml.XmlReader"/> to read</param>
        /// <param name="config">the <see cref="LX.EasyWeb.XmlRpc.IXmlRpcStreamConfig"/></param>
        /// <param name="typeFactory">the <see cref="LX.EasyWeb.XmlRpc.ITypeFactory"/> to create parsers</param>
        /// <returns>an parsed value</returns>
        /// <exception cref="System.Xml.XmlException">throws if error occurs during parsing</exception>
        public Object Read(System.Xml.XmlReader reader, IXmlRpcStreamConfig config, ITypeFactory typeFactory)
        {
            return Parse(reader.ReadElementContentAsString());
        }

        /// <summary>
        /// Parses the given string to a typed value.
        /// </summary>
        /// <param name="s">the string to parse</param>
        /// <returns>an parsed value</returns>
        protected abstract Object Parse(String s);
    }
}
