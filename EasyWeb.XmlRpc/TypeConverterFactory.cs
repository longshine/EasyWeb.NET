using System;
using System.Collections;

namespace LX.EasyWeb.XmlRpc
{
    class TypeConverterFactory : ITypeConverterFactory
    {
        static readonly ITypeConverter voidConverter = new IdentityTypeConverter(typeof(void));
        static readonly ITypeConverter mapConverter = new IdentityTypeConverter(typeof(IDictionary));
        static readonly ITypeConverter objectArrayConverter = new IdentityTypeConverter(typeof(Object[]));
        static readonly ITypeConverter byteArrayConverter = new IdentityTypeConverter(typeof(Byte[]));
        static readonly ITypeConverter stringConverter = new IdentityTypeConverter(typeof(String));
        static readonly ITypeConverter booleanConverter = new IdentityTypeConverter(typeof(Boolean));
        static readonly ITypeConverter charConverter = new IdentityTypeConverter(typeof(Char));
        static readonly ITypeConverter byteConverter = new IdentityTypeConverter(typeof(Byte));
        static readonly ITypeConverter floatConverter = new IdentityTypeConverter(typeof(Single));
        static readonly ITypeConverter doubleConverter = new IdentityTypeConverter(typeof(Double));
        static readonly ITypeConverter int16Converter = new IdentityTypeConverter(typeof(Int16));
        static readonly ITypeConverter int32Converter = new IdentityTypeConverter(typeof(Int32));
        static readonly ITypeConverter int64Converter = new IdentityTypeConverter(typeof(Int64));
        static readonly ITypeConverter dateTimeConverter = new IdentityTypeConverter(typeof(DateTime));
        static readonly ITypeConverter hashtableConverter = new HashtableTypeConverter();
        static readonly ITypeConverter arrayListConverter = new ArrayListTypeConverter();

        public ITypeConverter GetTypeConverter(Type type)
        {
            if (type == typeof(void))
                return voidConverter;
            else if (type.IsAssignableFrom(typeof(String)))
                return stringConverter;
            else if (type.IsAssignableFrom(typeof(Boolean)))
                return booleanConverter;
            else if (type.IsAssignableFrom(typeof(Char)))
                return charConverter;
            else if (type.IsAssignableFrom(typeof(Byte)))
                return byteConverter;
            else if (type.IsAssignableFrom(typeof(Single)))
                return floatConverter;
            else if (type.IsAssignableFrom(typeof(Double)))
                return doubleConverter;
            else if (type.IsAssignableFrom(typeof(Int16)))
                return int16Converter;
            else if (type.IsAssignableFrom(typeof(Int32)))
                return int32Converter;
            else if (type.IsAssignableFrom(typeof(Int64)))
                return int64Converter;
            else if (type.IsAssignableFrom(typeof(DateTime)))
                return dateTimeConverter;
            else if (type.IsAssignableFrom(typeof(IDictionary)))
                return mapConverter;
            else if (type.IsAssignableFrom(typeof(Object[])))
                return objectArrayConverter;
            else if (type.IsAssignableFrom(typeof(Byte[])))
                return byteArrayConverter;
            else if (type.IsAssignableFrom(typeof(IList)))
                return arrayListConverter;
            else if (type.IsAssignableFrom(typeof(Hashtable)))
                return hashtableConverter;

            throw new NotSupportedException("Invalid parameter or result type: " + type);
        }

        class IdentityTypeConverter : ITypeConverter
        {
            private Type _type;

            public IdentityTypeConverter(Type type)
            {
                _type = type;
            }

            public Boolean IsConvertable(Object obj)
            {
                return obj == null || _type.IsAssignableFrom(obj.GetType());
            }

            public Object Convert(Object obj)
            {
                return obj;
            }
        }

        class ArrayListTypeConverter : ITypeConverter
        {
            private Type _type = typeof(IList);

            public Boolean IsConvertable(Object obj)
            {
                return obj == null || obj is IEnumerable;
            }

            public Object Convert(Object obj)
            {
                if (obj == null)
                    return null;
                else if (_type.IsAssignableFrom(obj.GetType()))
                    return obj;
                else
                {
                    ArrayList list = new ArrayList();
                    foreach (var item in (IEnumerable)obj)
                    {
                        list.Add(item);
                    }
                    list.TrimToSize();
                    return list;
                }
            }
        }

        class HashtableTypeConverter : ITypeConverter
        {
            private Type _type = typeof(IDictionary);

            public Boolean IsConvertable(Object obj)
            {
                return obj == null || obj is IDictionary;
            }

            public Object Convert(Object obj)
            {
                if (obj == null)
                    return null;
                else if (_type.IsAssignableFrom(obj.GetType()))
                    return obj;
                else
                {
                    return new Hashtable((IDictionary)obj);
                }
            }
        }
    }
}
