//
// LX.EasyWeb.XmlRpc.IXmlRpcResponse.cs
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
    /// Represents an XML-RPC response.
    /// </summary>
    public interface IXmlRpcResponse
    {
        /// <summary>
        /// Gets the result, or null if a fault occurs.
        /// </summary>
        Object Result { get; }
        /// <summary>
        /// Gets the fault occured, or null if everything is fine.
        /// </summary>
        IXmlRpcFault Fault { get; }
    }
}
