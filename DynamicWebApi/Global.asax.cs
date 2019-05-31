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
using Server.Service;
using Castle.MicroKernel.Registration;
using DynamicWebApi.Controllers;

namespace DynamicWebApi
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // 注册自定义工厂
            var container = new Castle.Windsor.WindsorContainer();

            //通过Castle组合BaseController和ICalculator接口
            container.Register(
                Component.For<CalculatorService, ICalculator>(),
                Component.For<DynamciApiInterceptor<ICalculator>>().LifestyleTransient(),
                Component.For<BaseController>().Proxy.AdditionalInterfaces(typeof(ICalculator))
                    .Interceptors<DynamciApiInterceptor<ICalculator>>().LifestyleTransient()
                    .Named("CalculatorC")
            );

            var dynamicControllerFactory = new DynamicControllerFactory(container);
            ControllerBuilder.Current.SetControllerFactory(dynamicControllerFactory);
                
        }
    }
}