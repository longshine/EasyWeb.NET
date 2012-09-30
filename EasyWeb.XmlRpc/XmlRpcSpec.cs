//
// LX.EasyWeb.XmlRpc.XmlRpcSpec.cs
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
using System.Text;

namespace LX.EasyWeb.XmlRpc
{
    static class XmlRpcSpec
    {
        public const String METHOD_CALL_TAG = "methodCall";
        public const String METHOD_RESPONSE_TAG = "methodResponse";

        public const String METHOD_NAME_TAG = "methodName";

        public const String PARAMS_TAG = "params";
        public const String PARAM_TAG = "param";
        public const String VALUE_TAG = "value";
        public const String INT_TAG = "int";
        public const String I4_TAG = "i4";
        public const String I8_TAG = "i8";
        public const String BOOLEAN_TAG = "boolean";
        public const String STRING_TAG = "string";
        public const String DOUBLE_TAG = "double";
        public const String DATETIME_ISO8601_TAG = "dateTime.iso8601";
        public const String BASE64_TAG = "base64";
        public const String ARRAY_TAG = "array";
        public const String DATA_TAG = "data";
        public const String STRUCT_TAG = "struct";
        public const String MEMBER_TAG = "member";
        public const String MEMBER_NAME_TAG = "name";

        public const String FAULT_TAG = "fault";
        public const String FAULT_CODE_TAG = "faultCode";
        public const String FAULT_STRING_TAG = "faultString";

        public static readonly Encoding DEFAULT_ENCODING = Encoding.UTF8;
    }
}
