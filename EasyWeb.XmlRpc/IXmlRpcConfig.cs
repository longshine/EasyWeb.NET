//
// LX.EasyWeb.XmlRpc.IXmlRpcConfig.cs
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
    /// Provides configuration for both server and client.
    /// </summary>
    public interface IXmlRpcConfig
    {
        /// <summary>
        /// Gets if extensions are enabled.
        /// </summary>
        Boolean EnabledForExtensions { get; }
        /// <summary>
        /// Gets if the tag &lt;int&gt; is used, otherwise &lt;i4&gt; is used.
        /// </summary>
        Boolean UseIntTag { get; }
        /// <summary>
        /// Gets if the tag &lt;string&gt; is used.
        /// </summary>
        Boolean UseStringTag { get; }
    }
}
