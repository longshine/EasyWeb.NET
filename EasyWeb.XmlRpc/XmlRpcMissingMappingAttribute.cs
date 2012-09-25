using System;
using System.Collections.Generic;
using System.Text;

namespace LX.EasyWeb.XmlRpc
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct)]
    public class XmlRpcMissingMappingAttribute : Attribute
    {
        public MissingMappingAction Action { get; set; }
    }
}
