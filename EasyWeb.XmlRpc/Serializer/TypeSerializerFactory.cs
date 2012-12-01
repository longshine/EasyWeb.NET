﻿//
// LX.EasyWeb.XmlRpc.Serializer.TypeSerializerFactory.cs
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
using System.Collections;

namespace LX.EasyWeb.XmlRpc.Serializer
{
    /// <summary>
    /// Default implementation of <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializerFactory"/>.
    /// </summary>
    public class TypeSerializerFactory : ITypeSerializerFactory
    {
        private static readonly Int32Serializer i4Serializer = new Int32Serializer();
        private static readonly Int64Serializer i8Serializer = new Int64Serializer();
        private static readonly BooleanSerializer boolSerializer = new BooleanSerializer();
        private static readonly DoubleSerializer doubleSerializer = new DoubleSerializer();
        private static readonly StringSerializer stringSerializer = new StringSerializer();
        private static readonly DateTime8601Serializer dateTimeSerializer = new DateTime8601Serializer();
        private static readonly Base64Serializer base64Serializer = new Base64Serializer();
        private static readonly ObjectArraySerializer arraySerializer = new ObjectArraySerializer();
        private static readonly StructSerializer structSerializer = new StructSerializer();
        private static readonly ObjectSerializer objectSerializer = new ObjectSerializer();

        /// <summary>
        /// Gets a <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializer"/> for a parameter or result object.
        /// </summary>
        /// <param name="config">the request configuration</param>
        /// <param name="namespaceURI">the namespace URI of the element containing the parameter or result</param>
        /// <param name="localName">the local name of the element containing the parameter or result</param>
        /// <returns>a <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializer"/></returns>
        public ITypeSerializer GetSerializer(IXmlRpcStreamConfig config, String namespaceURI, String localName)
        {
            if (XmlRpcSpec.INT_TAG.Equals(localName) || XmlRpcSpec.I4_TAG.Equals(localName))
                return i4Serializer;
            else if (XmlRpcSpec.I8_TAG.Equals(localName))
                return i8Serializer;
            else if (XmlRpcSpec.BOOLEAN_TAG.Equals(localName))
                return boolSerializer;
            else if (XmlRpcSpec.DOUBLE_TAG.Equals(localName))
                return doubleSerializer;
            else if (XmlRpcSpec.STRING_TAG.Equals(localName))
                return stringSerializer;
            else if (XmlRpcSpec.DATETIME_ISO8601_TAG.Equals(localName))
                return dateTimeSerializer;
            else if (XmlRpcSpec.BASE64_TAG.Equals(localName))
                return base64Serializer;
            else if (XmlRpcSpec.ARRAY_TAG.Equals(localName))
                return arraySerializer;
            else if (XmlRpcSpec.STRUCT_TAG.Equals(localName))
                return structSerializer;
            else if (String.Empty.Equals(localName))
                return stringSerializer;

            return null;
        }

        /// <summary>
        /// Gets a <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializer"/> for a parameter or result object.
        /// </summary>
        /// <param name="config">the request configuration</param>
        /// <param name="obj">the object to serialize</param>
        /// <returns>a <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializer"/></returns>
        public ITypeSerializer GetSerializer(IXmlRpcStreamConfig config, Object obj)
        {
            if (obj == null)
                throw new XmlRpcException("Null values aren't supported");
            else if (obj is Int32)
                return i4Serializer;
            else if (obj is Int64)
                return i8Serializer;
            else if (obj is Boolean)
                return boolSerializer;
            else if (obj is Double)
                return doubleSerializer;
            else if (obj is String)
                return stringSerializer;
            else if (obj is DateTime)
                return dateTimeSerializer;
            else if (obj is Byte[])
                return base64Serializer;
            else if (obj is IEnumerable)
                return arraySerializer;
            else if (obj is IDictionary)
                return structSerializer;
            else if (obj is Enum)
                return stringSerializer;
            else
                return objectSerializer;
        }
    }
}
