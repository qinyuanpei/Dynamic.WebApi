using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Winton.Extensions.Configuration.Consul;
using System.Threading;

namespace DynamicWebApi.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            CreateWebHostBuilder(args)
                .ConfigureAppConfiguration((hostingContext, builder) =>
                {
                    builder
                        .AddConsul(
                            "DynamicWebApi.Core/appsettings.json",
                            cts.Token,
                            options =>
                            {
                                options.ConsulConfigurationOptions = cco => { cco.Address = new Uri("http://127.0.0.1:8500"); };
                                options.Optional = true;
                                options.ReloadOnChange = true;
                                options.OnLoadException = exceptionContext => { exceptionContext.Ignore = true; };
                            })
                        .AddEnvironmentVariables();
                })
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
                
    }
}
