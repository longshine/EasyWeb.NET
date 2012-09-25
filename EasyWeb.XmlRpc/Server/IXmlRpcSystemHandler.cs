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

    public abstract class XmlRpcSystemHandlers : IXmlRpcSystemHandler
    {
        [XmlRpcMethod(Name = "system.listMethods", IntrospectionMethod = true,
            Description = "Return an array of all available XML-RPC methods on this service.")]
        public String[] GetListMethods()
        {
            return GetSystemHandler().GetListMethods();
        }

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
                throw new XmlRpcFaultException(881, "No metadata available for method: " + methodName);
            return sig;
        }

        [XmlRpcMethod(Name = "system.methodHelp", IntrospectionMethod = true,
           Description = "Given the name of a method, return a help string.")]
        public String GetMethodHelp(String methodName)
        {
            String help = GetSystemHandler().GetMethodHelp(methodName);
            if (help == null)
                throw new XmlRpcException("No help available for method: " + methodName);
            return help;
        }

        protected abstract IXmlRpcSystemHandler GetSystemHandler();
    }
}
