//
// LX.EasyWeb.XmlRpc.IXmlRpcHttpServerConfig.cs
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
    /// Provides configration for HTTP servers.
    /// </summary>
    public interface IXmlRpcHttpServerConfig : IXmlRpcServerConfig, IXmlRpcHttpConfig
    {
        /// <summary>
        /// Gets if HTTP "Keep Alive" is enabled.
        /// </summary>
        Boolean KeepAliveEnabled { get; }
        /// <summary>
        /// Gets if the server should add a "faultCause" element in an error response.
        /// </summary>
        Boolean EnabledForExceptions { get; }
    }
}
