using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using DynamicWebApi.Controllers;
using System.Web.Http.Dispatcher;
using Server.Service;
using System.Web.Http.Controllers;

namespace DynamicWebApi
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 注册自定义工厂
            var container = new Castle.Windsor.WindsorContainer();

            //通过Castle组合BaseController和ICalculator接口
            DynamicHttpControllerManager.GetInstance().RegisterType<CalculatorService, ICalculator>();
            DynamicHttpControllerManager.GetInstance().RegisterType<MessageService, IMessage>();
            //container.Register(
            //    // 注册ICalculator
            //    Component.For<CalculatorService, ICalculator>(),
            //    Component.For<DynamicApiInterceptor<ICalculator>>().LifestyleTransient(),
            //    Component.For<BaseController>().Proxy.AdditionalInterfaces(typeof(ICalculator))
            //        .Interceptors<DynamicApiInterceptor<ICalculator>>().LifestyleTransient()
            //        .Named("Calculator"),

            //    //注册IMessage
            //    Component.For<MessageService, IMessage>(),
            //    Component.For<DynamicApiInterceptor<IMessage>>().LifestyleTransient(),
            //    Component.For<BaseController>().Proxy.AdditionalInterfaces(typeof(IMessage))
            //        .Interceptors<DynamicApiInterceptor<IMessage>>().LifestyleTransient()
            //        .Named("Message")

            //);

            //var dynamicControllerFactory = new DynamicControllerFactory(container);
            //ControllerBuilder.Current.SetControllerFactory(dynamicControllerFactory);

            var configuration = GlobalConfiguration.Configuration;
            var dynamicControllerSelector = new DynamicHttpControllerSelector(configuration);
            var dynamicHttpControllerActivtor = new DynamicHttpControllerActivtor();
            var dynamicActionSelector = new DynamicHttpActionSelector();
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector), dynamicControllerSelector);
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpActionSelector), dynamicActionSelector);
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), dynamicHttpControllerActivtor);

            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}