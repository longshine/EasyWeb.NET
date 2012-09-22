//
// LX.EasyWeb.XmlRpc.TypeFactory.cs
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
using LX.EasyWeb.XmlRpc.Parser;

namespace LX.EasyWeb.XmlRpc
{
    /// <summary>
    /// Default implementation of a <see cref="LX.EasyWeb.XmlRpc.ITypeFactory"/>
    /// </summary>
    public class TypeFactory : ITypeFactory
    {
        private static readonly ITypeParser i4Parser = new PrimitiveParser<Int32>();
        private static readonly ITypeParser i8Parser = new PrimitiveParser<Int64>();
        private static readonly ITypeParser boolParser = new PrimitiveParser<Boolean>();
        private static readonly ITypeParser doubleParser = new PrimitiveParser<Double>();
        private static readonly ITypeParser stringParser = new PrimitiveParser<String>();
        private static readonly ITypeParser dateTimeParser = new DateTime8601Parser();
        private static readonly ITypeParser base64Parser = new Base64Parser();
        private static readonly ITypeParser arrayParser = new ObjectArrayParser();
        private static readonly ITypeParser structParser = new StructParser();

        /// <summary>
        /// Gets a <see cref="LX.EasyWeb.XmlRpc.Parser.ITypeParser"/> for a parameter or result object.
        /// </summary>
        /// <param name="config">the request configuration</param>
        /// <param name="namespaceURI">the namespace URI of the element containing the parameter or result</param>
        /// <param name="localName">the local name of the element containing the parameter or result</param>
        /// <returns>a <see cref="LX.EasyWeb.XmlRpc.Parser.ITypeParser"/></returns>
        public ITypeParser GetParser(IXmlRpcStreamConfig config, String namespaceURI, String localName)
        {
            if (XmlRpcSpec.INT_TAG.Equals(localName) || XmlRpcSpec.I4_TAG.Equals(localName))
                return i4Parser;
            else if (XmlRpcSpec.I8_TAG.Equals(localName))
                return i8Parser;
            else if (XmlRpcSpec.BOOLEAN_TAG.Equals(localName))
                return boolParser;
            else if (XmlRpcSpec.DOUBLE_TAG.Equals(localName))
                return doubleParser;
            else if (XmlRpcSpec.STRING_TAG.Equals(localName))
                return stringParser;
            else if (XmlRpcSpec.DATETIME_ISO8601_TAG.Equals(localName))
                return dateTimeParser;
            else if (XmlRpcSpec.BASE64_TAG.Equals(localName))
                return base64Parser;
            else if (XmlRpcSpec.ARRAY_TAG.Equals(localName))
                return arrayParser;
            else if (XmlRpcSpec.STRUCT_TAG.Equals(localName))
                return structParser;
            else if (String.Empty.Equals(localName))
                return stringParser;
            
            return null;
        }
    }
}
