using Castle.Windsor;
using DynamicWebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace DynamicWebApi
{
    public class DynamicHttpControllerSelector: DefaultHttpControllerSelector
    {
        private IWindsorContainer _container;
        private HttpConfiguration _configuration;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public DynamicHttpControllerSelector(HttpConfiguration configuration, IWindsorContainer container) :
            base(configuration)
        {
            _container = container;
            _configuration = configuration;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData().Values;
            var serviceName = string.Empty;
            if (routeData.ContainsKey("serviceName"))
            {
                serviceName = routeData["serviceName"].ToString();
                var controller = _container.Resolve(serviceName, typeof(BaseController));
                return new DynamicHttpControllerDescriptor(_configuration, serviceName, controller.GetType());
            }

            return base.SelectController(request);
        }
    }
}