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
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public DynamicHttpControllerSelector(HttpConfiguration configuration):
            base(configuration)
        {

        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData().Values;
            var serviceName = string.Empty;
            if (routeData.ContainsKey("serviceName"))
                serviceName = routeData["serviceName"].ToString();
            var actionName = string.Empty;
            if (routeData.ContainsKey(actionName))
                actionName = routeData["actionName"].ToString();


            return base.SelectController(request);
        }
    }
}