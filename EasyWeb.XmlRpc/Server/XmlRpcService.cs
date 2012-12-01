
namespace LX.EasyWeb.XmlRpc.Server
{
    public class XmlRpcService : XmlRpcSystemHandlers
    {
        private static TypeReflectiveHandlerMapping mapping = new TypeReflectiveHandlerMapping();

        public XmlRpcService()
        {
            mapping.Register(this.GetType());
        }

        protected override IXmlRpcSystemHandler GetSystemHandler()
        {
            return mapping;
        }
    }
}
