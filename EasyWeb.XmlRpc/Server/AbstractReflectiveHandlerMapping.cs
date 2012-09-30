//
// LX.EasyWeb.XmlRpc.Server.AbstractReflectiveHandlerMapping.cs
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
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LX.EasyWeb.XmlRpc.Server
{
    abstract class AbstractReflectiveHandlerMapping : IXmlRpcListableHandlerMapping
    {
        private IDictionary<String, IXmlRpcHandler> _handlers = new Dictionary<String, IXmlRpcHandler>();

        public AbstractReflectiveHandlerMapping()
        {
            TypeConverterFactory = new TypeConverterFactory();
        }

        public IAuthenticationHandler AuthenticationHandler { get; set; }

        public ITypeConverterFactory TypeConverterFactory { get; set; }

        public IXmlRpcTargetProviderFactory TargetProviderFactory { get; set; }

        public Boolean VoidMethodEnabled { get; set; }

        public IXmlRpcHandler GetHandler(String name)
        {
            if (_handlers.ContainsKey(name))
                return _handlers[name];
            else
                throw new XmlRpcException("No such handler: " + name);
        }

        public String[] GetListMethods()
        {
            List<String> list = new List<String>();
            foreach (var pair in _handlers)
            {
                if (pair.Value is IXmlRpcMetaDataHandler)
                    list.Add(pair.Key);
            }
            return list.ToArray();
        }

        public String[][] GetMethodSignature(String name)
        {
            IXmlRpcHandler handler = GetHandler(name);
            return handler is IXmlRpcMetaDataHandler ? ((IXmlRpcMetaDataHandler)handler).Signatures : null;
        }

        public String GetMethodHelp(String name)
        {
            IXmlRpcHandler handler = GetHandler(name);
            return handler is IXmlRpcMetaDataHandler ? ((IXmlRpcMetaDataHandler)handler).MethodHelp : null;
        }

        protected void RegisterPublicMethods(String key, Type type)
        {
            XmlRpcServiceAttribute serviceAttr = (XmlRpcServiceAttribute)Attribute.GetCustomAttribute(type, typeof(XmlRpcServiceAttribute), true);
            if (key == null)
                key = (serviceAttr == null) ? type.Name : serviceAttr.Name;

            Dictionary<String, MethodInfo[]> map = new Dictionary<String, MethodInfo[]>();

            foreach (Type itf in type.GetInterfaces())
            {
                foreach (MethodInfo mi in itf.GetMethods())
                {
                    RegisterMethod(map, mi, key, type);
                }
            }

            foreach (MethodInfo mi in type.GetMethods())
            {
                RegisterMethod(map, mi, key, type);
            }

            foreach (var pair in map)
            {
                _handlers[pair.Key] = NewXmlRpcHandler(type, pair.Value);
            }
        }

        private void RegisterMethod(Dictionary<String, MethodInfo[]> map, MethodInfo mi, String key, Type type)
        {
            if (!IsHandlerMethod(mi))
                return;
            XmlRpcMethodAttribute methodAttr = (XmlRpcMethodAttribute)Attribute.GetCustomAttribute(mi, typeof(XmlRpcMethodAttribute), true);
            if (methodAttr == null)
                return;
            String name = (methodAttr == null || String.IsNullOrEmpty(methodAttr.Name)) ? (key + "." + mi.Name) :
                (methodAttr.Name.IndexOf('.') >= 0 ? methodAttr.Name : (key + "." + methodAttr.Name));
            MethodInfo[] mis;
            if (map.ContainsKey(name))
            {
                MethodInfo[] oldMis = map[name];
                if (HasDuplicateMethod(oldMis, mi))
                    throw new XmlRpcException(String.Format("Duplicate XML-RPC name {0} for method {1} in type {2}.", name, mi.Name, type.Name));
                else
                {
                    mis = new MethodInfo[oldMis.Length + 1];
                    oldMis.CopyTo(mis, 0);
                    mis[oldMis.Length] = mi;
                }
            }
            else
                mis = new MethodInfo[] { mi };

            map[name] = mis;
        }

        private Boolean HasDuplicateMethod(MethodInfo[] mis, MethodInfo mi)
        {
            ParameterInfo[] pis = mi.GetParameters();
            ParameterInfo[] existedPis;
            foreach (var existedMi in mis)
            {
                existedPis = existedMi.GetParameters();
                if (pis.Length == existedPis.Length)
                {
                    for (Int32 i = 0; i < pis.Length; i++)
                    {
                        String piType = Util.GetSignatureType(pis[i].ParameterType);
                        String existedPiType = Util.GetSignatureType(existedPis[i].ParameterType);
                        if (piType != null && existedPiType != null && !piType.Equals(existedPiType))
                            return false;
                        else if (pis[i].ParameterType != existedPis[i].ParameterType)
                            return false;
                    }
                }
            }
            return true;
        }

        private IXmlRpcHandler NewXmlRpcHandler(Type type, MethodInfo[] mis)
        {
            String[][] sig = Util.GetSignature(mis);
            String help = Util.GetMethodHelp(type, mis);
            IXmlRpcTargetProvider provider = TargetProviderFactory == null ? null : TargetProviderFactory.GetTargetProvider(type);
            if (sig == null || help == null)
                return new ReflectiveXmlRpcHandler(this, TypeConverterFactory, type, provider, mis);
            else
                return new ReflectiveXmlRpcMetaDataHandler(this, TypeConverterFactory, type, provider, mis, sig, help);
        }

        private Boolean IsHandlerMethod(MethodInfo mi)
        {
            if (!mi.IsPublic)
                return false;
            if (mi.IsStatic)
                return false;
            if (!VoidMethodEnabled && mi.ReturnType == typeof(void))
                return false;
            if (mi.DeclaringType == typeof(Object))
                return false;
            return true;
        }

        public interface IAuthenticationHandler
        {
            Boolean IsAuthorized(IXmlRpcRequest pRequest);
        }
    }

    class TypeReflectiveHandlerMapping : AbstractReflectiveHandlerMapping
    {
        public void Register(Type type)
        {
            RegisterPublicMethods(null, type);
        }
    }

    static class Util
    {
        public static String GetMethodHelp(Type type, MethodInfo[] mis)
        {
            List<String> list = new List<String>();
            foreach (var mi in mis)
            {
                String help = GetMethodHelp(type, mi);
                if (help != null)
                    list.Add(help);
            }
            if (list.Count == 0)
                return null;
            else if (list.Count == 1)
                return list[0];
            else
            {
                StringBuilder sb = new StringBuilder();
                for (Int32 i = 0; i < list.Count; i++)
                {
                    sb.Append(i + 1).Append(": ")
                        .Append(list[i]).AppendLine();
                }
                return sb.ToString();
            }
        }

        public static String GetMethodHelp(Type type, MethodInfo mi)
        {
            XmlRpcMethodAttribute methodAttr = (XmlRpcMethodAttribute)Attribute.GetCustomAttribute(mi, typeof(XmlRpcMethodAttribute), true);

            if (methodAttr != null && methodAttr.Hidden)
                return null;

            if (methodAttr != null)
            {
                if (methodAttr.Hidden)
                    return null;
                else if (!String.IsNullOrEmpty(methodAttr.Description))
                    return methodAttr.Description;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("Invokes the method ")
                .Append(type.Name).Append(".")
                .Append(mi.Name).Append("(");
            ParameterInfo[] pis = mi.GetParameters();
            for (Int32 i = 0; i < pis.Length; i++)
            {
                if (i > 0)
                    sb.Append(", ");
                sb.Append(pis[i].ParameterType.Name);
            }
            sb.Append(").");
            return sb.ToString();
        }

        public static String[][] GetSignature(MethodInfo[] mis)
        {
            List<String[]> list = new List<String[]>();
            foreach (var mi in mis)
            {
                String[] sig = GetSignature(mi);
                if (sig != null)
                    list.Add(sig);
            }
            return list.ToArray();
        }

        public static String[] GetSignature(MethodInfo mi)
        {
            XmlRpcMethodAttribute methodAttr = (XmlRpcMethodAttribute)Attribute.GetCustomAttribute(mi, typeof(XmlRpcMethodAttribute), true);

            if (methodAttr != null && methodAttr.Hidden)
                return null;

            ParameterInfo[] pis = mi.GetParameters();
            String[] sig = new String[pis.Length + 1];

            String s = GetSignatureType(mi.ReturnType);
            if (s == null)
                return null;
            sig[0] = s;
            for (Int32 i = 0; i < pis.Length; i++)
            {
                s = GetSignatureType(pis[i].ParameterType);
                if (s == null)
                    return null;
                sig[i + 1] = s;
            }
            return sig;
        }

        public static String GetSignatureType(Type type)
        {
            if (type == typeof(Int32) || type == typeof(Int64))
                return "int";
            else if (type == typeof(Double))
                return "double";
            else if (type == typeof(Boolean))
                return "boolean";
            else if (type == typeof(String))
                return "string";
            else if (typeof(IEnumerable).IsAssignableFrom(type))
                return "array";
            else if (typeof(IDictionary).IsAssignableFrom(type))
                return "struct";
            else if (type == typeof(DateTime))
                return "dateTime.iso8601";
            else if (type == typeof(Byte[]))
                return "base64";
            else
                return null;
        }

        public static String GetSignature(Object[] args)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < args.Length; i++)
            {
                if (i > 0)
                    sb.Append(", ");
                if (args[i] == null)
                    sb.Append("null");
                else
                    sb.Append(args[i].GetType().Name);
            }
            return sb.ToString();
        }
    }
}
