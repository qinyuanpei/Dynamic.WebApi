using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace DynamicWebApi
{
    public class DynamicHttpControllerDescriptor : HttpControllerDescriptor
    {

        public DynamicHttpControllerDescriptor(HttpConfiguration configuration, string controllerName, Type controllerType)
            : base(configuration, controllerName, controllerType)
        {

        }
    }
}