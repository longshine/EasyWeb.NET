//
// LX.EasyWeb.XmlRpc.XmlRpcConfig.cs
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
    abstract class XmlRpcConfig : IXmlRpcConfig, IXmlRpcHttpConfig
    {
        public Boolean EnabledForExtensions { get; set; }

        public String BasicEncoding { get; set; }

        public Boolean ContentLengthOptional { get; set; }

        public String Encoding { get; set; }
    }
}
