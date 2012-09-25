using System;

namespace LX.EasyWeb.XmlRpc
{
    public interface ITypeConverter
    {
        Boolean IsConvertable(Object obj);
        Object Convert(Object obj);
    }
}
