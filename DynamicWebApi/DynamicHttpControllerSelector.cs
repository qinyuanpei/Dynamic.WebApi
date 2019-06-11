using Castle.Windsor;
using DynamicWebApi.Controllers;
using Server.Service;
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
            if (routeData.ContainsKey("ServiceName") && routeData.ContainsKey("ActionName"))
            {
                var serviceName = routeData["ServiceName"].ToString();
                var actionName = routeData["ActionName"].ToString();

                if (DynamicHttpControllerManager.ContainsService(serviceName))
                {
                    var controllerInfo = DynamicHttpControllerManager.GetControllerInfo(serviceName);
                    var controller = _container.Resolve(serviceName, controllerInfo.ControllerType);
                    if (controller == null)
                        return base.SelectController(request);

                    var controllerType = controller.GetType();
                    var controllerDescriptor = new DynamicHttpControllerDescriptor(_configuration, serviceName, controllerInfo.ControllerType);
                    controllerDescriptor.Properties["ServiceName"] = serviceName;
                    controllerDescriptor.Properties["ActionName"] = actionName;
                    controllerDescriptor.Properties["IsDynamicController"] = true;
                    controllerDescriptor.Properties["ControllerType"] = controllerType;
                    return controllerDescriptor;
                }
                

                
            }

            return base.SelectController(request);
        }
    }
}