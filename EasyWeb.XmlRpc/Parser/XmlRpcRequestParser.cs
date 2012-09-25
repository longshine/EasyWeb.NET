//
// LX.EasyWeb.XmlRpc.Parser.XmlRpcRequestParser.cs
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
    public class XmlRpcRequestParser
    {
        private ITypeFactory _typeFactory;
        private IXmlRpcStreamConfig _config;
        private List<Object> _params;

        public XmlRpcRequestParser(IXmlRpcStreamConfig config, ITypeFactory typeFactory, XmlReader reader)
        {
            _config = config;
            _typeFactory = typeFactory;
            Parse(reader);
        }

        public String MethodName { get; private set; }

        public Object[] Parameters
        {
            get { return _params.ToArray(); }
        }

        private void Parse(XmlReader reader)
        {
            do
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (!String.IsNullOrEmpty(reader.NamespaceURI) || !XmlRpcSpec.METHOD_CALL_TAG.Equals(reader.LocalName))
                        throw new XmlException("Expected root element methodCall, got " + new XmlQualifiedName(reader.LocalName, reader.NamespaceURI));

                    ReadToElement(reader);
                    ReadMethodName(reader);
                    ReadToElement(reader);
                    ReadParams(reader);
                }
            } while (!reader.EOF && (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.METHOD_CALL_TAG.Equals(reader.LocalName)));
        }

        private void ReadParams(XmlReader reader)
        {
            if (reader.EOF)
                return;
            RecursiveTypeParser.CheckTag(reader, XmlRpcSpec.PARAMS_TAG);
            _params = new List<Object>();
            do
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element)
                {
                    _params.Add(ReadParam(reader));
                }
            } while (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.PARAMS_TAG.Equals(reader.LocalName));
        }

        private Object ReadParam(XmlReader reader)
        {
            RecursiveTypeParser.CheckTag(reader, XmlRpcSpec.PARAM_TAG);
            Object param = null;
            Int32 count = 0;
            do
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element && count++ == 0)
                {
                    param = RecursiveTypeParser.ReadValue(reader, _config, _typeFactory);
                }
            } while (reader.NodeType != XmlNodeType.EndElement || !XmlRpcSpec.PARAM_TAG.Equals(reader.LocalName));
            if (count == 0)
                throw new XmlException("No value element in the param element.");
            else if (count > 1)
                throw new XmlException("More than one value element in the param element.");
            return param;
        }

        private void ReadMethodName(XmlReader reader)
        {
            RecursiveTypeParser.CheckTag(reader, XmlRpcSpec.METHOD_NAME_TAG);
            MethodName = reader.ReadElementString();
        }

        private void ReadToElement(XmlReader reader)
        {
            while (reader.Read() && reader.NodeType != XmlNodeType.Element)
                ;
        }
    }
}
