//
// LX.EasyWeb.XmlRpc.Server.IXmlRpcHandlerMapping.cs
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

namespace LX.EasyWeb.XmlRpc.Server
{
    public interface IXmlRpcTargetProvider
    {
        Object GetTarget(IXmlRpcRequest request);
    }

    interface IXmlRpcTargetProviderFactory
    {
        IXmlRpcTargetProvider GetTargetProvider(Type type);
    }

    class StatefulTargetProviderFactory : IXmlRpcTargetProviderFactory
    {
        public IXmlRpcTargetProvider GetTargetProvider(Type type)
        {
            return new StatefulTargetProvider(type);
        }

        class StatefulTargetProvider : IXmlRpcTargetProvider
        {
            private Type _type;

            public StatefulTargetProvider(Type type)
            {
                _type = type;
            }

            public Object GetTarget(IXmlRpcRequest request)
            {
                return Activator.CreateInstance(_type);
            }
        }
    }

    class StatelessTargetProviderFactory : IXmlRpcTargetProviderFactory
    {
        public IXmlRpcTargetProvider GetTargetProvider(Type type)
        {
            return new StatelessTargetProvider(type);
        }

        class StatelessTargetProvider : IXmlRpcTargetProvider
        {
            private readonly Object _obj;

            public StatelessTargetProvider(Type type)
            {
                _obj = Activator.CreateInstance(type);
            }

            public Object GetTarget(IXmlRpcRequest request)
            {
                return _obj;
            }
        }
    }
}
