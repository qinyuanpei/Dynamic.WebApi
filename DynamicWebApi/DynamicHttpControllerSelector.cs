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
        private HttpConfiguration _configuration;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public DynamicHttpControllerSelector(HttpConfiguration configuration) :
            base(configuration)
        {
            _configuration = configuration;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData().Values;
            if (routeData.ContainsKey("ServiceName") && routeData.ContainsKey("ActionName"))
            {
                var serviceName = routeData["ServiceName"].ToString();
                var actionName = routeData["ActionName"].ToString();

                if (DynamicHttpControllerManager.GetInstance().ContainsService(serviceName))
                {
                    var controllerInfo = DynamicHttpControllerManager.GetInstance().GetControllerInfo(serviceName);
                    var controller = DynamicHttpControllerManager.GetInstance().Resolve(serviceName);
                    if (controller == null)
                        return base.SelectController(request);

                    var controllerDescriptor = new DynamicHttpControllerDescriptor(_configuration, serviceName, controllerInfo.ControllerType);
                    controllerDescriptor.Properties["ServiceName"] = serviceName;
                    controllerDescriptor.Properties["ActionName"] = actionName;
                    controllerDescriptor.Properties["IsDynamicController"] = true;
                    controllerDescriptor.Properties["ServiceType"] = controllerInfo.ServiceType;
                    controllerDescriptor.Properties["ControllerType"] = controller.GetType();
                    return controllerDescriptor;
                }
                 
            }

            return base.SelectController(request);
        }
    }
}