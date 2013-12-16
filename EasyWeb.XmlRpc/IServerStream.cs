//
// LX.EasyWeb.XmlRpc.IServerStream.cs
//
// Authors:
//	Longshine He <longshinehe@users.sourceforge.net>
//
// Copyright (c) 2012 Longshine He
//
// This code is distributed in the hope that it will be useful,
// but WITHOUT WARRANTY OF ANY KIND.
//

using System.IO;

namespace LX.EasyWeb.XmlRpc
{
    /// <summary>
    /// Represents a request stream and provides accessibility to both input and output streams.
    /// </summary>
    public interface IServerStream
    {
        /// <summary>
        /// Gets the input stream.
        /// </summary>
        Stream InputStream { get; }
        /// <summary>
        /// Gets the output stream.
        /// </summary>
        Stream OutputStream { get; }
    }
}
