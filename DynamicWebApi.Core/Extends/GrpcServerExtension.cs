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
        public static IServiceCollection AddGrpcServer(this IServiceCollection serviceCollection)
        {
            var server = new Server(new[] { new ChannelOption(ChannelOptions.SoReuseport, 5000) });
            var serverPort = new ServerPort("127.0.0.1", 5000, ServerCredentials.Insecure);
            server.Ports.Add(serverPort);
            serviceCollection.AddSingleton(typeof(Server), server);
            var channel = new Channel(host: serverPort.Host, port: serverPort.Port, credentials: ChannelCredentials.Insecure);
            serviceCollection.AddSingleton(typeof(Channel), channel);
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
