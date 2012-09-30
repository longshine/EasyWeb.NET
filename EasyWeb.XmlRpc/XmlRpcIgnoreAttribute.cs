//
// LX.EasyWeb.XmlRpc.XmlRpcIgnoreAttribute.cs
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
    /// Indicates that a field or property should be ignored during XML-RPC serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class XmlRpcIgnoreAttribute : Attribute
    {
    }
}
