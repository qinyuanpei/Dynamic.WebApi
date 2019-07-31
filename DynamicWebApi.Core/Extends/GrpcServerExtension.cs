using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using DynamicWebApi.Core.Services;
using Microsoft.AspNetCore.Builder;

namespace DynamicWebApi.Core.Extends
{
    public static class GrpcServerExtension
    {
        public static IServiceCollection AddGrpcServer(this IServiceCollection serviceCollection,GrpcServerOptions options)
        {
            var server = new Server();
            server.Ports.Add(new ServerPort(options.Host, options.Port, ServerCredentials.Insecure));
            serviceCollection.AddSingleton(typeof(Server), server);
            return serviceCollection;
        }

        public static IServiceCollection AddGrpcService<TService, TServiceImp>(this IServiceCollection serviceCollection)
        {
            var server = serviceCollection.GetService<Server>();
            server.Services.Add(GreetGrpc.GreeterGrpcService.BindService(new GreeterService()));
            return serviceCollection;
        }

        public static IServiceCollection AddGrpcService(this IServiceCollection serviceCollection)
        {
            var server = serviceCollection.GetService<Server>();
            server.Services.Add(GreetGrpc.GreeterGrpcService.BindService(new GreeterService()));
            return serviceCollection;
        }

        public static void UseGrpcServer(this IApplicationBuilder app)
        {
            var server = app.ApplicationServices.GetService<Server>();
            server.Start();
        }
    }
}
