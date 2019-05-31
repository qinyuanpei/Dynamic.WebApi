using System.Reflection;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;
using Server.Service;

namespace Abp.DynamicWebApi.Api
{
    [DependsOn(typeof(AbpWebApiModule), typeof(DynamicWebApiApplicationModule))]
    public class DynamicWebApiWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                //.For<ICalculator>("api/calculator")
                .ForAll<IApplicationService>(typeof(DynamicWebApiApplicationModule).Assembly, "app")
                .Build();

            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));
        }
    }
}
