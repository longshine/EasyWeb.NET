using System;

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
        Object Execute(IXmlRpcRequest request);
    }

    interface IXmlRpcMetaDataHandler : IXmlRpcHandler
    {
        String[][] Signatures { get; }
        String MethodHelp { get; }
    }
}
