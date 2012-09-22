//
// LX.EasyWeb.XmlRpc.IXmlRpcHttpConfig.cs
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
    /// Provides configuration for HTTP.
    /// </summary>
    public interface IXmlRpcHttpConfig : IXmlRpcStreamConfig
    {
        /// <summary>
        /// Gets the encoding being used for basic HTTP authentication credentials, or null if the default encoding.
        /// </summary>
        String BasicEncoding { get; }
        /// <summary>
        /// Gets if a "Content-Length" header may be omitted. The XML-RPC specification demands that such a header be present.
        /// </summary>
        Boolean ContentLengthOptional { get; }
    }
}
