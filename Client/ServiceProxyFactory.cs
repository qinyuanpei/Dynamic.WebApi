using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Channels;

namespace Client
{
    public static class ServiceProxyFactory
    {
        public static TService CreatePorxy<TService>(Binding binding, string serviceUrl)
        {
            var dynamicProxy = new DynamicServiceProxy<TService>(binding, serviceUrl);
            return (TService)dynamicProxy.GetTransparentProxy();
        }
    }
}
