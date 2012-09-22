//
// LX.EasyWeb.XmlRpc.IXmlRpcStreamConfig.cs
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
    /// Provides configuration for a stream based transport.
    /// </summary>
    public interface IXmlRpcStreamConfig : IXmlRpcConfig
    {
        /// <summary>
        /// Gets the data encoding of the stream.
        /// </summary>
        String Encoding { get; }
    }
}
