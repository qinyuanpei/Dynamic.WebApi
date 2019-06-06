using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;

namespace DynamicWebApi
{
    public class DynamicHttpActionDescriptor : ReflectedHttpActionDescriptor
    {
        public DynamicHttpActionDescriptor(HttpControllerDescriptor controllerDescriptor, MethodInfo methodInfo)
            : base(controllerDescriptor, methodInfo)
        {

        }
    }
}