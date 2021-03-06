﻿//
// LX.EasyWeb.XmlRpc.IXmlRpcRequest.cs
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
    /// Represents an XML-RPC request.
    /// </summary>
    public interface IXmlRpcRequest
    {
        /// <summary>
        /// Gets the request method name.
        /// </summary>
        String MethodName { get; }
        /// <summary>
        /// Gets the request parameters.
        /// </summary>
        Object[] Parameters { get; }
        /// <summary>
        /// Gets the request target object.
        /// </summary>
        Object Target { get; set; }
    }
}
