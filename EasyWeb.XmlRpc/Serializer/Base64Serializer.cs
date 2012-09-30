//
// LX.EasyWeb.XmlRpc.Serializer.Base64Serializer.cs
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

namespace LX.EasyWeb.XmlRpc.Serializer
{
    class Base64Serializer : StringSerializer
    {
        protected override String GetTag(IXmlRpcStreamConfig config)
        {
            return XmlRpcSpec.BASE64_TAG;
        }

        protected override Object DoRead(System.Xml.XmlReader reader)
        {
            return Convert.FromBase64String((String)base.DoRead(reader));
        }
    }
}
