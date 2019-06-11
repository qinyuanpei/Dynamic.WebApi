using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DynamicWebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicWebApi
{
    public class DynamicHttpControllerManager
    {
        /// <summary>
        /// Castle容器
        /// </summary>
        private readonly IWindsorContainer _container;

        /// <summary>
        /// 控制器类型信息
        /// </summary>
        private readonly static Dictionary<string, DynamicControllerInfo> _controllerInfoList 
            = new Dictionary<string, DynamicControllerInfo>();

        public DynamicHttpControllerManager(IWindsorContainer container)
        {
            _container = container;
        }

        public void RegisterType<TImplement, TInterface>(string serviceName = "")
        {
            if (string.IsNullOrEmpty(serviceName))
                serviceName = typeof(TImplement).Name;

            _container.Register(
                Component.For(typeof(TImplement), typeof(TInterface)),
                Component.For<DynamicApiInterceptor<TInterface>>().LifestyleTransient(),
                Component.For<BaseController<TInterface>>().Proxy.AdditionalInterfaces(typeof(TInterface))
                    .Interceptors<DynamicApiInterceptor<TInterface>>().LifestyleTransient()
                    .Named(serviceName)
            );

            _controllerInfoList.Add(serviceName, new DynamicControllerInfo(typeof(TInterface)));
        }

        public static bool ContainsService(string serviceName)
        {
            return _controllerInfoList.ContainsKey(serviceName);
        }

        public static bool ContainsService<T>()
        {
            var serviceName = typeof(T).Name;
            return _controllerInfoList.ContainsKey(serviceName);
        }

        public static DynamicControllerInfo GetControllerInfo(string serviceName)
        {
            return _controllerInfoList[serviceName];
        }
    }
}