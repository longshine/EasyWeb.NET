//
// LX.EasyWeb.XmlRpc.Parser.StructParser.cs
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
    class StructParser : RecursiveTypeParser
    {
        protected override Object DoReadResult(XmlReader reader, IXmlRpcStreamConfig config, ITypeFactory typeFactory)
        {
            IDictionary<String, Object> dic = new Dictionary<String, Object>();

            do
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (!String.IsNullOrEmpty(reader.NamespaceURI) || !XmlRpcSpec.MEMBER_TAG.Equals(reader.LocalName))
                        ThrowHelper.ThrowUnexpectedTag(XmlRpcSpec.MEMBER_TAG, reader);
                    do
                    {
                        reader.Read();
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (!String.IsNullOrEmpty(reader.NamespaceURI) || !XmlRpcSpec.MEMBER_NAME_TAG.Equals(reader.LocalName))
                                ThrowHelper.ThrowUnexpectedTag(XmlRpcSpec.MEMBER_NAME_TAG, reader);

                            String name = reader.ReadElementContentAsString();
                            if (name == null)
                                throw new XmlException("Name is expected for struct members.");

                            while (reader.NodeType != XmlNodeType.Element && reader.Read())
                                ;

                            if (dic.ContainsKey(name))
                                throw new XmlException("Duplicate struct member name: " + name);

                            Object value = ReadValue(reader, config, typeFactory);
                            dic.Add(name, value);
                        }
                    } while (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.MEMBER_TAG.Equals(reader.LocalName));
                }
            } while (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.STRUCT_TAG.Equals(reader.LocalName));

            return dic;
        }
    }
}
