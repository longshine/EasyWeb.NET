//
// LX.EasyWeb.XmlRpc.Parser.ObjectArrayParser.cs
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
using System.Collections.Generic;
using System.Xml;

namespace LX.EasyWeb.XmlRpc.Parser
{
    class ObjectArrayParser : RecursiveTypeParser
    {
        protected override Object DoReadResult(XmlReader reader, IXmlRpcStreamConfig config, ITypeFactory typeFactory)
        {
            List<Object> list = new List<Object>();

            do
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (!String.IsNullOrEmpty(reader.NamespaceURI) || !XmlRpcSpec.DATA_TAG.Equals(reader.LocalName))
                        ThrowHelper.ThrowUnexpectedTag(XmlRpcSpec.DATA_TAG, reader);
                    do
                    {
                        reader.Read();
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            list.Add(ReadValue(reader, config, typeFactory));
                        }
                    } while (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.DATA_TAG.Equals(reader.LocalName));
                }
            } while (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.ARRAY_TAG.Equals(reader.LocalName));

            return list.ToArray();
        }
    }
}
