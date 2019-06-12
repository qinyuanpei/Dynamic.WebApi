using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DynamicWebApi.Controllers;
using Server.Service;
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
        /// 单例模式下线程安全锁
        /// </summary>
        private static readonly object _lockObj = new object();

        /// <summary>
        /// 控制器类型信息
        /// </summary>
        private readonly static Dictionary<string, DynamicControllerInfo> _controllerInfoList 
            = new Dictionary<string, DynamicControllerInfo>();

        /// <summary>
        /// 构造函数
        /// </summary>
        private DynamicHttpControllerManager()
        {
            _container = new Castle.Windsor.WindsorContainer();
        }

        /// <summary>
        /// 单例模式
        /// </summary>
        private static DynamicHttpControllerManager _instance = null;
        public static DynamicHttpControllerManager GetInstance()
        {
            if(_instance == null)
            {
                lock (_lockObj)
                {
                    if (_instance == null)
                        _instance = new DynamicHttpControllerManager();

                    return _instance;
                }
            }

            return _instance;
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <typeparam name="TImplement">服务实现</typeparam>
        /// <typeparam name="TInterface">服务接口</typeparam>
        /// <param name="serviceName">服务名称</param>
        public void RegisterType<TImplement, TInterface>(string serviceName = "")
        {
            if (string.IsNullOrEmpty(serviceName))
                serviceName = GetServiceName<TImplement>();

            _container.Register(
                Component.For(typeof(TImplement), typeof(TInterface)),
                Component.For<DynamicApiInterceptor<TInterface>>().LifestyleTransient(),
                Component.For<BaseController<TInterface>>().Proxy.AdditionalInterfaces(typeof(TInterface))
                    .Interceptors<DynamicApiInterceptor<TInterface>>().LifestyleTransient()
                    .Named(serviceName)
            );

            _controllerInfoList.Add(serviceName, new DynamicControllerInfo(typeof(TInterface)));
        }

        /// <summary>
        /// 是否包含执行服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public bool ContainsService(string serviceName)
        {
            return _controllerInfoList.ContainsKey(serviceName);
        }

        /// <summary>
        /// 是否包含执行服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public bool ContainsService<T>()
        {
            var serviceName = typeof(T).Name;
            return _controllerInfoList.ContainsKey(serviceName);
        }

        /// <summary>
        /// 获取指定服务对应得控制器信息
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public DynamicControllerInfo GetControllerInfo(string serviceName)
        {
            return _controllerInfoList[serviceName];
        }

        /// <summary>
        /// 返回指定服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public object Resolve(string serviceName)
        {
            if (_controllerInfoList.ContainsKey(serviceName))
            {
                var controllerInfo = _controllerInfoList[serviceName];
                return _container.Resolve(serviceName,controllerInfo.ControllerType);
            }

            return null;
        }

        /// <summary>
        /// 获取指定名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        /// <summary>
        /// 获取服务名称
        /// </summary>
        /// <typeparam name="TImplement">服务实现</typeparam>
        /// <returns></returns>
        private string GetServiceName<TImplement>()
        {
            var typeName = typeof(TImplement).Name;
            if (!typeName.EndsWith("Service"))
                return typeName;
            else
                return typeName.Substring(0, typeName.IndexOf("Service"));
        }
    }
}