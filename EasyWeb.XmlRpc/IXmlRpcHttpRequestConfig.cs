//
// LX.EasyWeb.XmlRpc.IXmlRpcHttpRequestConfig.cs
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
    /// Provides configuration like user credentials for a HTTP based transport.
    /// </summary>
    public interface IXmlRpcHttpRequestConfig : IXmlRpcStreamRequestConfig, IXmlRpcHttpConfig
    {
        /// <summary>
        /// Gets the user name used for basic HTTP authentication.
        /// </summary>
        String BasicUserName { get; }
        /// <summary>
        /// Gets the password used for basic HTTP authentication.
        /// </summary>
        String BasicPassword { get; }
        /// <summary>
        /// Gets the connection timeout in milliseconds or 0 if no set.
        /// </summary>
        Int32 ConnectionTimeout { get; }
        /// <summary>
        /// Gets the reply timeout in milliseconds or 0 if no set.
        /// </summary>
        Int32 ReplyTimeout { get; }
    }
}
