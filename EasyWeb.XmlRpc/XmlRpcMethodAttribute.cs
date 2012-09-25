using System;

namespace LX.EasyWeb.XmlRpc
{
    [AttributeUsage(AttributeTargets.Method)]
    public class XmlRpcMethodAttribute : Attribute
    {
        public String Name { get; set; }

        public Boolean IntrospectionMethod { get; set; }

        public Boolean StructParams { get; set; }

        public String Description { get; set; }

        public Boolean Hidden { get; set; }
    }
}
