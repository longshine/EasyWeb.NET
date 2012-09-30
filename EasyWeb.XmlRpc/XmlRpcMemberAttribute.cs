//
// LX.EasyWeb.XmlRpc.XmlRpcMemberAttribute.cs
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
    /// Determines how a field or property can be serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class XmlRpcMemberAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of this member in an XML-RPC request/response.
        /// </summary>
        public String Name { get; set; }
    }
}
