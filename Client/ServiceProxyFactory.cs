using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Channels;
using Castle.DynamicProxy;

namespace Client
{
    public static class ServiceProxyFactory
    {
        public static TService CreatePorxy<TService>(Binding binding, string serviceUrl)
        {
            var dynamicProxy = new DynamicServiceProxy<TService>(binding, serviceUrl);
            return (TService)dynamicProxy.GetTransparentProxy();
        }

        public static TService CreateCastleProxy<TService>(Binding binding, string serviceUrl) 
        {
            ProxyGenerator generator = new ProxyGenerator();
            var interceptor = new CastleServicePorxy<TService>(binding, serviceUrl);
            return (TService)generator.CreateInterfaceProxyWithoutTarget(typeof(TService),interceptor);
        }
    }
}
