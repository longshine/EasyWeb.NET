//
// LX.EasyWeb.XmlRpc.Parser.Base64Parser.cs
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

namespace LX.EasyWeb.XmlRpc.Parser
{
    class Base64Parser : AtomicParser
    {
        protected override Object Parse(String s)
        {
            return Convert.FromBase64String(s);
        }
    }
}
