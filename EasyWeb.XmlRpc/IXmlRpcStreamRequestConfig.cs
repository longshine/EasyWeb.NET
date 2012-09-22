//
// LX.EasyWeb.XmlRpc.IXmlRpcStreamRequestConfig.cs
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
    /// Represents a stream request configuration.
    /// </summary>
    public interface IXmlRpcStreamRequestConfig : IXmlRpcStreamConfig, IXmlRpcRequestConfig
    {
        /// <summary>
        /// Gets if the request stream is being compressed.
        /// </summary>
        Boolean GzipCompressing { get; }
        /// <summary>
        /// Gets if compression is requested for the response stream.
        /// </summary>
        Boolean GzipRequesting { get; }
        /// <summary>
        /// Gets if the response should contain a "faultCause" element in case of errors.
        /// </summary>
        Boolean EnabledForExceptions { get; }
    }
}
