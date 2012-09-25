using System;

namespace LX.EasyWeb.XmlRpc
{
    public interface ITypeConverterFactory
    {
        ITypeConverter GetTypeConverter(Type type);
    }
}
