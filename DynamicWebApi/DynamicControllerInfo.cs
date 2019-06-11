using DynamicWebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicWebApi
{
    public class DynamicControllerInfo
    {
        private readonly Type _serviceType;

        public DynamicControllerInfo(Type serviceType)
        {
            _serviceType = serviceType;
        }

        /// <summary>
        /// 服务类型
        /// </summary>
        public Type ServiceType => _serviceType;

        /// <summary>
        /// 控制器类型
        /// </summary>
        public Type ControllerType => typeof(BaseController<>).MakeGenericType(_serviceType);

        /// <summary>
        /// 拦截器类型
        /// </summary>
        public Type InterceptorType => typeof(DynamicApiInterceptor<>).MakeGenericType(_serviceType);

    }
}