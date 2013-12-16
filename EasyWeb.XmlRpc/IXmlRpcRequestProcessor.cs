//
// LX.EasyWeb.XmlRpc.IXmlRpcRequestProcessor.cs
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

namespace LX.EasyWeb.XmlRpc
{
    /// <summary>
    /// Represents an object which is able to process XML-RPC requests.
    /// </summary>
    public interface IXmlRpcRequestProcessor
    {
        /// <summary>
        /// Processes the given request and returns a result object.
        /// </summary>
        Object Execute(IXmlRpcRequest request);
        /// <summary>
        /// Gets the request processor's <see cref="TypeConverterFactory"/>
        /// </summary>
        ITypeConverterFactory TypeConverterFactory { get; }
    }
}
