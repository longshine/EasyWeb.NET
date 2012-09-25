using System;
using System.Web;

namespace LX.EasyWeb.XmlRpc.Server
{
    public class XmlRpcServiceHandler : XmlRpcHttpServer, IHttpHandler
    {
        private TypeReflectiveHandlerMapping _mapping = new TypeReflectiveHandlerMapping();

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
            return _mapping.GetHandler(request.Method);
        }
    }
}
