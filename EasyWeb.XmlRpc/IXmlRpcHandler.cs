using System;
using System.Reflection;

namespace LX.EasyWeb.XmlRpc
{
    /// <summary>
    /// Represents a XML-RPC handler.
    /// </summary>
    public interface IXmlRpcHandler
    {
        /// <summary>
        /// Performs the request and returns the result object.
        /// </summary>
        /// <param name="request">the request being performed</param>
        /// <returns>the result object</returns>
        /// <exception cref="LX.EasyWeb.XmlRpc.XmlRpcException">an exception occurs when handling the request</exception>
        Object Execute(IXmlRpcRequest request);
        /// <summary>
        /// Gets the request method.
        /// </summary>
        /// <param name="request">the request being performed</param>
        /// <returns>the returned <see cref="System.Reflection.MethodInfo"/></returns>
        MethodInfo GetMethod(IXmlRpcRequest request);
    }

    interface IXmlRpcMetaDataHandler : IXmlRpcHandler
    {
        String[][] Signatures { get; }
        String MethodHelp { get; }
    }
}
