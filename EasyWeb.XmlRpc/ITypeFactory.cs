//
// LX.EasyWeb.XmlRpc.ITypeFactory.cs
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
using LX.EasyWeb.XmlRpc.Parser;

namespace LX.EasyWeb.XmlRpc
{
    /// <summary>
    /// Provides methods to get type serializers.
    /// </summary>
    public interface ITypeFactory
    {
        /// <summary>
        /// Gets a <see cref="LX.EasyWeb.XmlRpc.Parser.ITypeParser"/> for a parameter or result object.
        /// </summary>
        /// <param name="config">the request configuration</param>
        /// <param name="namespaceURI">the namespace URI of the element containing the parameter or result</param>
        /// <param name="localName">the local name of the element containing the parameter or result</param>
        /// <returns>a <see cref="LX.EasyWeb.XmlRpc.Parser.ITypeParser"/></returns>
        ITypeParser GetParser(IXmlRpcStreamConfig config, String namespaceURI, String localName);
    }
}
