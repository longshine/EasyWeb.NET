//
// LX.EasyWeb.XmlRpc.Server.XmlRpcSystemHandlers.cs
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

namespace LX.EasyWeb.XmlRpc.Server
{
    /// <summary>
    /// Provides methods that implements the XML-RPC introspection.
    /// </summary>
    public abstract class XmlRpcSystemHandlers : MarshalByRefObject, IXmlRpcSystemHandler
    {
        /// <summary>
        /// This method implements the introspection method <code>system.listMethods</code>.
        /// </summary>
        [XmlRpcMethod(Name = "system.listMethods", IntrospectionMethod = true,
            Description = "Return an array of all available XML-RPC methods on this service.")]
        public String[] GetListMethods()
        {
            return GetSystemHandler().GetListMethods();
        }

        /// <summary>
        /// This method implements the introspection method <code>system.methodSignature</code>.
        /// </summary>
        [XmlRpcMethod(Name = "system.methodSignature", IntrospectionMethod = true,
           Description =
           "Given the name of a method, return an array of legal signatures. " +
           "Each signature is an array of strings. The first item of each " +
           "signature is the return type, and any others items are parameter " +
           "types.")]
        public String[][] GetMethodSignature(String methodName)
        {
            String[][] sig = GetSystemHandler().GetMethodSignature(methodName);
            if (sig == null)
                throw new XmlRpcException("No metadata available for method: " + methodName);
            return sig;
        }

        /// <summary>
        /// This method implements the introspection method <code>system.methodHelp</code>.
        /// </summary>
        [XmlRpcMethod(Name = "system.methodHelp", IntrospectionMethod = true,
           Description = "Given the name of a method, return a help string.")]
        public String GetMethodHelp(String methodName)
        {
            String help = GetSystemHandler().GetMethodHelp(methodName);
            if (help == null)
                throw new XmlRpcException("No help available for method: " + methodName);
            return help;
        }

        /// <summary>
        /// Gets the <see cref="LX.EasyWeb.XmlRpc.Server.IXmlRpcSystemHandler"/> that actually implements these introspection methods.
        /// </summary>
        /// <returns></returns>
        protected abstract IXmlRpcSystemHandler GetSystemHandler();
    }
}
