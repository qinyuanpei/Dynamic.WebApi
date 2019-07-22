using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicWebApi.Core.Extends
{
    public static class DynamicControllerServiceExtension
    {
        public static IServiceCollection AddDyanmicController(this IServiceCollection services, DynamicControllerOptions options = null)
        {
            var partManager = services.GetService<ApplicationPartManager>();
            if (partManager == null)
            {
                throw new InvalidOperationException("请在AddMvc()方法后调用AddDynamicController()");
            }

            partManager.FeatureProviders.Add(new DynamicControllerFeatureProvider());
            if (options == null) options = DynamicControllerOptions.Default;
            services.AddSingleton(typeof(DynamicControllerOptions), options);
            services.Configure<MvcOptions>(o =>
            {
                o.Conventions.Add(new DynamicControllerConvention(services));
            });

            return services;
        }

        public static TService GetService<TService>(this IServiceCollection services) where TService : class
        {
            var service = services.FirstOrDefault(s => s.ServiceType == typeof(TService));
            if (service != null)
                return service.ImplementationInstance as TService;

            return null;
        }
    }
}
