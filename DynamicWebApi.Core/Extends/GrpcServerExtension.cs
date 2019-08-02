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
        /// <summary>
        /// 添加GrpcServer
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddGrpcServer(this IServiceCollection serviceCollection, GrpcServerOptions options = null)
        {
            if (options == null)
                options = new GrpcServerOptions() { Host = "0.0.0.0", Port = 2345 };

            var server = new Server();
            var serverPort = new ServerPort(options.Host, options.Port, ServerCredentials.Insecure);
            server.Ports.Add(serverPort);
            var channel = new Channel($"localhost:{options.Port}", credentials: ChannelCredentials.Insecure);

            serviceCollection.AddSingleton(typeof(Server), server);
            serviceCollection.AddSingleton(typeof(Channel), channel);
            return serviceCollection;
        }

        /// <summary>
        /// 添加Grpc服务
        /// </summary>
        /// <typeparam name="TServiceImp"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddGrpcService<TServiceImp>(this IServiceCollection serviceCollection) where TServiceImp:class
        {
            var server = serviceCollection.GetService<Server>();
            var serviceType = (typeof(TServiceImp).GetCustomAttributes(typeof(GrpcServiceBindAttribute), false)[0] as GrpcServiceBindAttribute).BindType;
            var bindMethod = serviceType.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                .Where(m=>m.Name == "BindService" && m.ReturnType == typeof(ServerServiceDefinition)).FirstOrDefault();
            if (bindMethod == null)
                throw new Exception($"The specify service \"{serviceType.Name}\" is not a gRpc service with \"BindService()\"");

            //注入ServiceImp
            serviceCollection.AddTransient<TServiceImp>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var serviceImp = serviceProvider.GetService<TServiceImp>();
            if (serviceImp == null)
                throw new Exception($"The sprcift service \"{typeof(TServiceImp).Name}\" must be registered in DI container");

            var serviceDefine = bindMethod.Invoke(null, new object[] { serviceImp }) as ServerServiceDefinition;
            server.Services.Add(serviceDefine);
            return serviceCollection;
        }

        /// <summary>
        /// 添加Grpc客户端
        /// </summary>
        /// <typeparam name="TServiceClient"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddGrpcClient<TServiceClient>(this IServiceCollection serviceCollection) where TServiceClient:class
        {
            serviceCollection.AddTransient<TServiceClient>();
            return serviceCollection;
        }

        public static void UseGrpcServer(this IApplicationBuilder app)
        {
            var server = app.ApplicationServices.GetService<Server>();
            server.Start();
            server.Ports.ToList().ForEach(a => Console.WriteLine($"GrpcServer now is listening on  http://localhost:{a.Port}..."));
        }
    }
}
