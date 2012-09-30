//
// LX.EasyWeb.XmlRpc.Serializer.TypeSerializer.cs
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

namespace LX.EasyWeb.XmlRpc.Serializer
{
    /// <summary>
    /// Default implementation of <see cref="LX.EasyWeb.XmlRpc.Serializer.ITypeSerializer"/>.
    /// </summary>
    public abstract class TypeSerializer : ITypeSerializer
    {
        public Object Read(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            return DoRead(reader, config, typeSerializerFactory);
        }

        public void Write(XmlWriter writer, Object obj, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            DoWrite(writer, obj, config, typeSerializerFactory);
        }

        protected abstract Object DoRead(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory);
        protected abstract void DoWrite(XmlWriter writer, Object obj, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory);
    }
}
