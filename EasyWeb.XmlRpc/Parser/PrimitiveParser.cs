//
// LX.EasyWeb.XmlRpc.Parser.PrimitiveParser.cs
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
using System.Xml;

namespace LX.EasyWeb.XmlRpc.Parser
{
    class PrimitiveParser<T> : ITypeParser
    {
        private static Type type = typeof(T);

        public Object Read(XmlReader reader, IXmlRpcStreamConfig config, ITypeFactory typeFactory)
        {
            return reader.NodeType == XmlNodeType.Text ? reader.ReadContentAs(type, null) : reader.ReadElementContentAs(type, null);
        }
    }
}
