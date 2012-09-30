//
// LX.EasyWeb.XmlRpc.Serializer.ITypeSerializerFactory.cs
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

namespace LX.EasyWeb.XmlRpc.Serializer
{
    /// <summary>
    /// Provides methods to get type serializers.
    /// </summary>
    public interface ITypeSerializerFactory
    {
        /// <summary>
        /// Gets a <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializer"/> for a parameter or result object.
        /// </summary>
        /// <param name="config">the request configuration</param>
        /// <param name="namespaceURI">the namespace URI of the element containing the parameter or result</param>
        /// <param name="localName">the local name of the element containing the parameter or result</param>
        /// <returns>a <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializer"/></returns>
        ITypeSerializer GetSerializer(IXmlRpcStreamConfig config, String namespaceURI, String localName);
        /// <summary>
        /// Gets a <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializer"/> for a parameter or result object.
        /// </summary>
        /// <param name="config">the request configuration</param>
        /// <param name="obj">the object to serialize</param>
        /// <returns>a <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializer"/></returns>
        ITypeSerializer GetSerializer(IXmlRpcStreamConfig config, Object obj);
    }
}
