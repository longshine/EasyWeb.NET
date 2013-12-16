//
// LX.EasyWeb.XmlRpc.Serializer.ObjectSerializer.cs
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
using System.Reflection;
using System.Xml;

namespace LX.EasyWeb.XmlRpc.Serializer
{
    class ObjectSerializer : RecursiveTypeSerializer
    {
        protected override Object DoRead(XmlReader reader, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory)
        {
            throw new InvalidOperationException("Cannot deserialize objects directly, using StructSerializer instead.");
        }

        protected override void DoWrite(XmlWriter writer, Object obj, IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory, IList nestedObjs)
        {
            writer.WriteStartElement(XmlRpcSpec.VALUE_TAG);
            writer.WriteStartElement(XmlRpcSpec.STRUCT_TAG);

            Type type = obj.GetType();

            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                WriteMember(writer, obj, pi, config, typeSerializerFactory, nestedObjs);
            }

            foreach (FieldInfo fi in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                WriteMember(writer, obj, fi, config, typeSerializerFactory, nestedObjs);
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        private void WriteMember(XmlWriter writer, Object obj, MemberInfo mi,
            IXmlRpcStreamConfig config, ITypeSerializerFactory typeSerializerFactory, IList nestedObjs)
        {
            if (Attribute.IsDefined(mi, typeof(XmlRpcIgnoreAttribute)))
                return;

            Object value = null;
            if (mi is PropertyInfo)
                value = ((PropertyInfo)mi).GetValue(obj, null);
            else if (mi is FieldInfo)
                value = ((FieldInfo)mi).GetValue(obj);

            MissingMemberAction action = config.MissingMemberAction;
            // TODO check member attribute

            XmlRpcMemberAttribute ma = (XmlRpcMemberAttribute)Attribute.GetCustomAttribute(mi, typeof(XmlRpcMemberAttribute));
            String memberName = (ma != null && !String.IsNullOrEmpty(ma.Name)) ? ma.Name : mi.Name;

            if (value == null)
            {
                if (action == MissingMemberAction.Ignore)
                    return;
                else if (action == MissingMemberAction.Error)
                    throw new XmlRpcException("Missing non-optional member: " + memberName);
            }

            writer.WriteStartElement(XmlRpcSpec.MEMBER_TAG);
            writer.WriteElementString(XmlRpcSpec.MEMBER_NAME_TAG, memberName);
            WriteValue(writer, value, config, typeSerializerFactory, nestedObjs);
            writer.WriteEndElement();
        }
    }
}
