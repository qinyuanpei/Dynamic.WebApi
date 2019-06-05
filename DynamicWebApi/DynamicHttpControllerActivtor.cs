using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace DynamicWebApi
{
    public class DynamicHttpControllerActivtor : IHttpControllerActivator
    {
        private IWindsorContainer _container;
        public DynamicHttpControllerActivtor(IWindsorContainer container)
        {
            _container = container;

        }
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            return (IHttpController)_container.Resolve(controllerType);
        }
    }
}