//
// LX.EasyWeb.XmlRpc.XmlRpcHttpRequestConfig.cs
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
    public class XmlRpcHttpRequestConfig : XmlRpcConfig, IXmlRpcHttpRequestConfig
    {
        public String BasicUserName { get; set; }

        public String BasicPassword { get; set; }

        public Int32 ConnectionTimeout { get; set; }

        public Int32 ReplyTimeout { get; set; }

        public Boolean GzipCompressing { get; set; }

        public Boolean GzipRequesting { get; set; }

        public Boolean EnabledForExceptions { get; set; }
    }
}
