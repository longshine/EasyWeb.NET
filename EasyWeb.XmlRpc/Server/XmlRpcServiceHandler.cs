//
// LX.EasyWeb.XmlRpc.Server.XmlRpcServiceHandler.cs
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
using System.Web;

namespace LX.EasyWeb.XmlRpc.Server
{
    /// <summary>
    /// Represnets a HTTP handler that provides XML-RPC methods.
    /// </summary>
    public class XmlRpcServiceHandler : XmlRpcHttpServer, IHttpHandler
    {
        private TypeReflectiveHandlerMapping _mapping = new TypeReflectiveHandlerMapping();

        /// <summary>
        /// Initializes.
        /// </summary>
        public XmlRpcServiceHandler()
        {
            _mapping.Register(this.GetType());
        }

        public Boolean IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HandleHttpRequest(context.Request, context.Response);
        }

        protected override IXmlRpcSystemHandler GetSystemHandler()
        {
            return _mapping;
        }

        protected override IXmlRpcHandler GetHandler(IXmlRpcRequest request)
        {
            request.Target = this;
            return _mapping.GetHandler(request.MethodName);
        }
    }
}
