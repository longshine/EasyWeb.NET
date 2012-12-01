using System;

namespace LX.EasyWeb.XmlRpc
{
    public enum MissingMemberAction
    {
        Ignore,
        Error
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Struct
        | AttributeTargets.Property | AttributeTargets.Class)]
    public class XmlRpcMissingMemberAttribute : Attribute
    {
        public XmlRpcMissingMemberAttribute(MissingMemberAction action)
        {
            Action = action;
        }

        public MissingMemberAction Action { get; private set; }
    }
}
