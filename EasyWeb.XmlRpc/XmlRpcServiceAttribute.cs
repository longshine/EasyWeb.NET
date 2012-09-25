using System;

namespace LX.EasyWeb.XmlRpc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class XmlRpcServiceAttribute : Attribute
    {
        public String Name { get; set; }
    }
}
