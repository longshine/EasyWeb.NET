//
// LX.EasyWeb.XmlRpc.Serializer.IXmlWriterFactory.cs
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
using System.IO;
using System.Text;
using System.Xml;

namespace LX.EasyWeb.XmlRpc.Serializer
{
    /// <summary>
    /// Provides methods to get <see cref="System.Xml.XmlWriter"/>.
    /// </summary>
    public interface IXmlWriterFactory
    {
        /// <summary>
        /// Gets a <see cref="System.Xml.XmlWriter"/> with given <see cref="LX.EasyWeb.XmlRpc.IXmlRpcStreamConfig"/> and <see cref="System.IO.Stream"/>.
        /// </summary>
        /// <param name="config">the <see cref="LX.EasyWeb.XmlRpc.IXmlRpcStreamConfig"/></param>
        /// <param name="stream">the <see cref="System.IO.Stream"/> to write</param>
        /// <returns>an <see cref="System.Xml.XmlWriter"/></returns>
        XmlWriter GetXmlWriter(IXmlRpcStreamConfig config, Stream stream);
    }

    class XmlTextWriterFactory : IXmlWriterFactory
    {
        public XmlWriter GetXmlWriter(IXmlRpcStreamConfig config, Stream stream)
        {
            Encoding encoding = String.IsNullOrEmpty(config.Encoding) ? XmlRpcSpec.DEFAULT_ENCODING : Encoding.GetEncoding(config.Encoding);
            XmlTextWriter writer = new XmlTextWriter(stream, encoding);
            return writer;
        }
    }
}
