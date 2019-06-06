using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;

namespace DynamicWebApi
{
    public class DynamicHttpActionSelector : ApiControllerActionSelector
    {
        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            var isDynamicController = controllerContext.ControllerDescriptor.Properties.ContainsKey("IsDynamicController");
            if (isDynamicController)
            {
                var controllerType = new object();
                if (controllerContext.ControllerDescriptor.Properties.TryGetValue("ControllerType", out controllerType))
                {
                    var actionName = controllerContext.ControllerDescriptor.Properties["ActionName"].ToString();
                    var methodInfo = ((Type)controllerType).GetMethod(actionName);
                    return new DynamicHttpActionDescriptor(controllerContext.ControllerDescriptor, methodInfo);
                }
            }

            return base.SelectAction(controllerContext);
        }
    }
}