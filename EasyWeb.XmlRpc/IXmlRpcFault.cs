//
// LX.EasyWeb.XmlRpc.IXmlRpcFault.cs
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
    /// Represents an XML-RPC fault.
    /// </summary>
    public interface IXmlRpcFault
    {
        /// <summary>
        /// Gets the fault code.
        /// </summary>
        Int32 FaultCode { get; }
        /// <summary>
        /// Gets the fault string.
        /// </summary>
        String FaultString { get; }
    }
}
