//
// LX.EasyWeb.XmlRpc.Server.IXmlRpcHandlerMapping.cs
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
    public interface IXmlRpcSystemHandler
    {
        /// <summary>
        /// This method implements the introspection method <code>system.listMethods</code>.
        /// </summary>
        /// <returns></returns>
        String[] GetListMethods();
        /// <summary>
        /// This method implements the introspection method <code>system.methodSignature</code>.
        /// </summary>
        String[][] GetMethodSignature(String name);
        /// <summary>
        /// This method implements the introspection method <code>system.methodHelp</code>.
        /// </summary>
        String GetMethodHelp(String name);
    }
}
