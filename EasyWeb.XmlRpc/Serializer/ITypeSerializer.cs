//
// LX.EasyWeb.XmlRpc.Serializer.ITypeSerializer.cs
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

namespace LX.EasyWeb.XmlRpc.Serializer
{
    /// <summary>
    /// Provides methods serialize/deserialize objects from a <see cref="System.Xml.XmlReader"/>.
    /// </summary>
    public interface ITypeSerializer
    {
        /// <summary>
        /// Reads a typed value.
        /// </summary>
        /// <param name="reader">the <see cref="System.Xml.XmlReader"/> to read from</param>
        /// <param name="config">the <see cref="LX.EasyWeb.XmlRpc.IXmlRpcStreamConfig"/></param>
        /// <param name="typeSerializerFactory">the <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializerFactory"/> to create serializers</param>
        /// <returns>an parsed value</returns>
        /// <exception cref="System.Xml.XmlException">throws if error occurs during parsing</exception>
        Object Read(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory);
        /// <summary>
        /// Writes an object.
        /// </summary>
        /// <param name="writer">the <see cref="System.Xml.XmlWriter"/> to write to</param>
        /// <param name="obj">the object to write</param>
        /// <param name="config">the <see cref="LX.EasyWeb.XmlRpc.IXmlRpcStreamConfig"/></param>
        /// <param name="typeSerializerFactory">the <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializerFactory"/> to create serializers</param>
        void Write(XmlWriter writer, Object obj, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory);
    }
}
