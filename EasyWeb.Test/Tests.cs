using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using LX.EasyWeb.XmlRpc;
using LX.EasyWeb.XmlRpc.Serializer;

namespace LX.EasyWeb.Test
{
    class Tests
    {
        public void ParseMissingMethodCall()
        {
            String xmlrpcRequest = @"<methodCalling></methodCalling>";

            Exception ex = null;
            try
            {
                Parse(xmlrpcRequest);
            }
            catch (Exception e)
            {
                ex = e;
            }
            Assert.IsNotNull(ex);
            Assert.IsEqualTo(ex.Message, "Expected root element methodCall, got methodCalling");
        }

        public void ParseMissingMethodName()
        {
            String xmlrpcRequest = @"<methodCall>
        <params>
            <param><value><i4>3</i4></value></param>
        </params>
    </methodCall>";

            Exception ex = null;
            try
            {
                Parse(xmlrpcRequest);
            }
            catch (Exception e)
            {
                ex = e;
            }
            Assert.IsNotNull(ex);
            Assert.IsEqualTo(ex.Message, "Expected methodName element, got params");
        }

        public void ParseMissingParams()
        {
            String xmlrpcRequest = @"<methodCall>
        <methodName>sample</methodName>
    </methodCall>";

            IXmlRpcRequest request = Parse(xmlrpcRequest);
            Assert.IsEqualTo(request.MethodName, "sample");
        }

        [ActiveTest]
        public void ParseEmptyParams()
        {
            String xmlrpcRequest = @"<methodCall>
  <methodName>sample</methodName>
  <params/>
</methodCall>";

            IXmlRpcRequest request = Parse(xmlrpcRequest);
            Assert.IsEqualTo(request.MethodName, "sample");
        }

        public void ParseEmptyValue()
        {
            String xmlrpcRequest = @"<methodCall>
        <methodName>sample</methodName>
        <params>
            <param>
                <value></value>
            </param>
        </params>
    </methodCall>";
            IXmlRpcRequest request = Parse(xmlrpcRequest);
            Assert.IsNull(request.Parameters[0]);
        }

        public void ParseMalformedValue()
        {
            String xmlrpcRequest = @"<methodCall>
        <methodName>sample</methodName>
        <params>
            <param>
                <value><i4><shoudnotbehere/></i4></value>
            </param>
        </params>
    </methodCall>";
            Exception ex = null;
            try
            {
                IXmlRpcRequest request = Parse(xmlrpcRequest);
            }
            catch (Exception e)
            {
                ex = e;
            }
            Assert.IsNotNull(ex);
        }

        public void ParseMalformedMultiValuesInParams()
        {
            String xmlrpcRequest = @"<methodCall>
        <methodName>sample</methodName>
        <params>
            <param>
                <value><i4>1</i4></value>
                <value><i4>2</i4></value>
            </param>
        </params>
    </methodCall>";

            Exception ex = null;
            try
            {
                Parse(xmlrpcRequest);
            }
            catch (Exception e)
            {
                ex = e;
            }
            Assert.IsNotNull(ex);
            Assert.IsEqualTo(ex.Message, "More than one value element in the param element.");
        }

        public void ParseMalformedNoValueInParams()
        {
            String xmlrpcRequest = @"<methodCall>
        <methodName>sample</methodName>
        <params>
            <param>
            </param>
        </params>
    </methodCall>";

            Exception ex = null;
            try
            {
                Parse(xmlrpcRequest);
            }
            catch (Exception e)
            {
                ex = e;
            }
            Assert.IsNotNull(ex);
            Assert.IsEqualTo(ex.Message, "No value element in the param element.");
        }

        public void ParseMalformedParams()
        {
            String xmlrpcRequest = @"<methodCall>
        <methodName>sample</methodName>
        <params>
            <param><hi></hi>
            </param>
        </params>
    </methodCall>";

            Exception ex = null;
            try
            {
                Parse(xmlrpcRequest);
            }
            catch (Exception e)
            {
                ex = e;
            }
            Assert.IsNotNull(ex);
            Assert.IsEqualTo(ex.Message, "Expected value element, got hi");
        }

        public void ParseUnknownTypeName()
        {
            String xmlrpcRequest = @"<methodCall>
        <methodName>sample</methodName>
        <params>
            <param><value><i16>3</i16></value></param>
        </params>
    </methodCall>";
            
            Exception ex = null;
            try
            {
                Parse(xmlrpcRequest);
            }
            catch (Exception e)
            {
                ex = e;
            }
            Assert.IsNotNull(ex);
            Assert.IsEqualTo(ex.Message, "Unknown type: i16");
        }

        public void ParseDefaultTypeName()
        {
            String xmlrpcRequest = @"<methodCall>
        <methodName>sample</methodName>
        <params>
            <param><value>default string</value></param>
            <param><value>
                <array>
                   <data>
                      <value>default string in array</value>
                    </data>
                </array>
            </value></param>
            <param><value>
                <struct>
                    <member>
                        <name>name</name>
                        <value>default string in struct</value>
                    </member>
                </struct>
            </value></param>
        </params>
    </methodCall>";

            IXmlRpcRequest request = Parse(xmlrpcRequest);

            Assert.IsEqualTo(request.Parameters[0], "default string");
            Assert.IsEqualTo(((Object[])request.Parameters[1])[0], "default string in array");
            Assert.IsEqualTo(((IDictionary<String, Object>)request.Parameters[2])["name"], "default string in struct");
        }

        public void ParseMalformedMultiDataInArray()
        {
            String xmlrpcRequest = @"<methodCall>
        <methodName>sample</methodName>
        <params>
            <param><value>
                <array>
                   <data>
                      <value><i4>12</i4></value>
                    </data>
                   <data>
                      <value><i4>-31</i4></value>
                    </data>
                </array>
            </value></param>
        </params>
    </methodCall>";

            IXmlRpcRequest request = null;
            Exception ex = null;
            try
            {
                request = Parse(xmlrpcRequest);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.IsEqualTo(((Object[])request.Parameters[0])[0], 12);
            Assert.IsEqualTo(((Object[])request.Parameters[0])[1], -31);
            //Assert.IsNotNull(ex);
            //Assert.IsEqualTo(ex.Message, "More than one data element in the array element.");
        }

        public void ParseMalformedMultiEntriesInValue()
        {
            String xmlrpcRequest = @"<methodCall>
        <methodName>sample</methodName>
        <params>
            <param><value>
                <i4>1</i4>
                <i4>2</i4>
            </value></param>
        </params>
    </methodCall>";

            IXmlRpcRequest request = null;
            Exception ex = null;
            try
            {
                request = Parse(xmlrpcRequest);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.IsEqualTo(request.Parameters[0], 2);
            //Assert.IsNotNull(ex);
            //Assert.IsEqualTo(ex.Message, "More than one data element in the array element.");
        }
        
        public void ParseMalformedMissingNameInStruct()
        {
            String xmlrpcRequest = @"<methodCall>
        <methodName>sample</methodName>
        <params>
            <param><value>
                <struct>
                   <member>
                      <value><i4>12</i4></value>
                    </member>
                </struct>
            </value></param>
        </params>
    </methodCall>";

            IXmlRpcRequest request = null;
            Exception ex = null;
            try
            {
                request = Parse(xmlrpcRequest);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.IsNotNull(ex);
            Assert.IsEqualTo(ex.Message, "Expected name element, got value");
        }

        public void ParseMalformedMissingValueInStruct()
        {
            String xmlrpcRequest = @"<methodCall>
        <methodName>sample</methodName>
        <params>
            <param><value>
                <struct>
                   <member>
                      <name>m1</name>
                    </member>
                </struct>
            </value></param>
        </params>
    </methodCall>";

            IXmlRpcRequest request = null;
            Exception ex = null;
            try
            {
                request = Parse(xmlrpcRequest);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.IsNotNull(ex);
            Assert.IsEqualTo(ex.Message, "Expected value element, got ");
        }

        public void ParseDuplicateMemberNameInStruct()
        {
            String xmlrpcRequest = @"<methodCall>
        <methodName>sample</methodName>
        <params>
            <param><value>
                <struct>
                   <member>
                      <name>m1</name><value></value>
                    </member>
                   <member>
                      <name>m1</name><value></value>
                    </member>
                </struct>
            </value></param>
        </params>
    </methodCall>";

            IXmlRpcRequest request = null;
            Exception ex = null;
            try
            {
                request = Parse(xmlrpcRequest);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.IsNotNull(ex);
            Assert.IsEqualTo(ex.Message, "Duplicate struct member name: m1");
        }

        public void ParseRecursiveData()
        {
            String xmlrpcRequest = @"<methodCall>
        <methodName>sample</methodName>
        <params>
            <param><value>
                <struct>
                   <member>
                      <name>arrayInStruct</name>
                      <value>
                        <array><data><value><i4>1</i4></value><value><i4>2</i4></value></data></array>
                      </value>
                    </member>
                </struct>
            </value></param>
            <param><value>
                <array>
                 <data><value>
                   <struct><member>
                      <name>arrayInStructInArray</name>
                      <value>
                        <array><data><value><i4>3</i4></value><value><i4>4</i4></value></data></array>
                      </value>
                    </member></struct>
                 </value></data>
                </array>
            </value></param>
        </params>
    </methodCall>";

            IXmlRpcRequest request = null;
            Exception ex = null;
            try
            {
                request = Parse(xmlrpcRequest);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.IsNull(ex);
            Assert.IsEqualTo(((Object[])((IDictionary<String, Object>)request.Parameters[0])["arrayInStruct"])[0], 1);
            Assert.IsEqualTo(((Object[])((IDictionary<String, Object>)request.Parameters[0])["arrayInStruct"])[1], 2);
            Object[] array = (Object[])request.Parameters[1];
            Assert.IsEqualTo(((Object[])((IDictionary<String, Object>)array[0])["arrayInStructInArray"])[0], 3);
            Assert.IsEqualTo(((Object[])((IDictionary<String, Object>)array[0])["arrayInStructInArray"])[1], 4);
        }

        public void ParseXmlRpcRequest()
        {
            String xmlrpcRequest = @"<methodCall>
        <methodName>sample</methodName>
        <params>
            <param><value>eW91IGNhbid0IHJlYWQgdGhpcyE=</value></param>
            <param><value><i4>3</i4></value></param>
            <param><value><int>-7</int></value></param>
            <param><value><i8>1234567890</i8></value></param>
            <param><value><boolean>1</boolean></value></param>
            <param><value><boolean>false</boolean></value></param>
            <param><value><string>hello xmlrpc</string></value></param>
            <param><value><double>-3.145</double></value></param>
            <param><value><dateTime.iso8601>19980717T14:08:55</dateTime.iso8601></value></param>
            <param><value><base64>eW91IGNhbid0IHJlYWQgdGhpcyE=</base64></value></param>
            <param><value>
                <array>
                   <data>
                      <value><i4>12</i4></value>
                      <value><string>Egypt</string></value>
                      <value><boolean>0</boolean></value>
                      <value><i4>-31</i4></value>
                    </data>
                </array>
            </value></param>
            <param><value>
                <struct>
                    <member>
                        <name>lowerBound</name>
                        <value><i4>18</i4></value>
                    </member>
                    <member>
                        <name>upperBound</name>
                        <value><i4>139</i4></value>
                    </member>
                </struct>
            </value></param>
        </params>
    </methodCall>";
            XmlTextReader reader = new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(xmlrpcRequest)));
            //IXmlRpcRequest request = new XmlRpcRequestSerializer(null, new TypeFactory(), reader);
            XmlRpcRequestSerializer parser = new XmlRpcRequestSerializer();
            IXmlRpcRequest request = parser.ReadRequest(reader, null, new TypeSerializerFactory());

            Assert.IsEqualTo(request.MethodName, "sample");
            Assert.IsEqualTo(request.Parameters[0], "eW91IGNhbid0IHJlYWQgdGhpcyE=");
            Assert.IsEqualTo(request.Parameters[1], 3);
            Assert.IsEqualTo(request.Parameters[2], -7);
            Assert.IsEqualTo(request.Parameters[3], 1234567890L);
            Assert.IsEqualTo(request.Parameters[4], true);
            Assert.IsEqualTo(request.Parameters[5], false);
            Assert.IsEqualTo(request.Parameters[6], "hello xmlrpc");
            Assert.IsEqualTo(request.Parameters[7], -3.145D);
            Assert.IsEqualTo(request.Parameters[8], new DateTime(1998, 7, 17, 14, 8, 55));
            Assert.IsSequenceEqualTo((Byte[])request.Parameters[9], Convert.FromBase64String("eW91IGNhbid0IHJlYWQgdGhpcyE="));

            Object[] array = (Object[])request.Parameters[10];
            Assert.IsEqualTo(array[0], 12);
            Assert.IsEqualTo(array[1], "Egypt");
            Assert.IsEqualTo(array[2], false);
            Assert.IsEqualTo(array[3], -31);

            IDictionary<String, Object> map = (IDictionary<String, Object>)request.Parameters[11];
            Assert.IsEqualTo(map["lowerBound"], 18);
            Assert.IsEqualTo(map["upperBound"], 139);
            //Assert.IsEqualTo(request.Parameters[10],);
        }

        public void ParseXmlRpcResponse()
        {
            String xmlrpc = @"
<methodResponse>
   <params>
      <param>
         <value><string>South Dakota</string></value>
         </param>
      </params>
   </methodResponse>";

            XmlTextReader reader = new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(xmlrpc)));
            XmlRpcResponseSerializer serializer = new XmlRpcResponseSerializer();
            IXmlRpcResponse request = serializer.ReadResponse(reader, null, new TypeSerializerFactory());
            Assert.IsNull(request.Fault);
            Assert.IsEqualTo(request.Result, "South Dakota");
        }

        public void ParseXmlRpcFaultResponse()
        {
            String xmlrpc = @"
<methodResponse>
   <fault>
      <value>
         <struct>
            <member>
               <name>faultCode</name>
               <value><int>4</int></value>
               </member>
            <member>
               <name>faultString</name>
               <value><string>Too many parameters.</string></value>
               </member>
            </struct>
         </value>
      </fault>
   </methodResponse>
";

            XmlTextReader reader = new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(xmlrpc)));
            XmlRpcResponseSerializer serializer = new XmlRpcResponseSerializer();
            IXmlRpcResponse request = serializer.ReadResponse(reader, null, new TypeSerializerFactory());
            Assert.IsNull(request.Result);
            Assert.IsNotNull(request.Fault);
            Assert.IsEqualTo(request.Fault.FaultCode, 4);
            Assert.IsEqualTo(request.Fault.FaultString, "Too many parameters.");
        }

        public void WriteXmlRpcResponse()
        {
            XmlRpcResponse response = new XmlRpcResponse("South Dakota");
            StringBuilder sb = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb));
            XmlRpcResponseSerializer serializer = new XmlRpcResponseSerializer();
            serializer.WriteResponse(writer, response, null, new TypeSerializerFactory());
            Assert.IsEqualTo(sb.ToString(), @"<?xml version=""1.0"" encoding=""utf-16""?><methodResponse><params><param><value><string>South Dakota</string></value></param></params></methodResponse>");
        }

        public void WriteXmlRpcFaultResponse()
        {
            XmlRpcResponse response = new XmlRpcResponse(null);
            response.Fault = new XmlRpcFault(4, "Too many parameters.");
            StringBuilder sb = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb));
            XmlRpcResponseSerializer serializer = new XmlRpcResponseSerializer();
            serializer.WriteResponse(writer, response, null, new TypeSerializerFactory());
            Assert.IsEqualTo(sb.ToString(), @"<?xml version=""1.0"" encoding=""utf-16""?><methodResponse><fault><value><struct><member><name>faultCode</name><value><i4>4</i4></value></member><member><name>faultString</name><value><string>Too many parameters.</string></value></member></struct></value></fault></methodResponse>");
        }

        public void WriteXmlRpcRequest()
        {
            XmlRpcRequest request = new XmlRpcRequest("examples.getStateName", new Object[] { 41, "32" });
            StringBuilder sb = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb));
            XmlRpcRequestSerializer serializer = new XmlRpcRequestSerializer();
            serializer.WriteRequest(writer, request, null, new TypeSerializerFactory());//.WriteRequest(writer, request, null, new TypeSerializerFactory());
            Assert.IsEqualTo(sb.ToString(), @"<?xml version=""1.0"" encoding=""utf-16""?><methodCall><methodName>examples.getStateName</methodName><params><param><value><i4>41</i4></value></param><param><value><string>32</string></value></param></params></methodCall>");
        }

        private static IXmlRpcRequest Parse(String xmlrpcRequest)
        {
            XmlTextReader reader = new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(xmlrpcRequest)));
            XmlRpcRequestSerializer parser = new XmlRpcRequestSerializer();
            return parser.ReadRequest(reader, null, new TypeSerializerFactory());
        }
    }
}
