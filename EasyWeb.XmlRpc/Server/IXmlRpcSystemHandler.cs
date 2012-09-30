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
