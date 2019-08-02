using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using DynamicWebApi.Core.Services;
using Microsoft.AspNetCore.Builder;
using Consul;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Sockets;

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
            serviceCollection.AddSingleton(typeof(Server), server);
            return serviceCollection;
        }

        /// <summary>
        /// 添加Grpc服务
        /// </summary>
        /// <typeparam name="TServiceImp"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddGrpcService<TServiceImp>(this IServiceCollection serviceCollection) where TServiceImp : class
        {
            var server = serviceCollection.GetService<Server>();
            var serviceType = (typeof(TServiceImp).GetCustomAttributes(typeof(GrpcServiceBindAttribute), false)[0] as GrpcServiceBindAttribute).BindType;
            var bindMethod = serviceType.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                .Where(m => m.Name == "BindService" && m.ReturnType == typeof(ServerServiceDefinition)).FirstOrDefault();
            if (bindMethod == null)
                throw new Exception($"The specify service \"{serviceType.Name}\" is not a gRpc service with \"BindService()\"");

            //注入ServiceImp
            serviceCollection.AddTransient<TServiceImp>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var serviceImp = serviceProvider.GetService<TServiceImp>();
            if (serviceImp == null)
                throw new Exception($"The sprcift service \"{typeof(TServiceImp).Name}\" must be registered in DI container");

            //注册Consul
            RegisterConsul<TServiceImp>(server, serviceCollection).Wait();

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
        public static IServiceCollection AddGrpcClient<TServiceClient>(this IServiceCollection serviceCollection) where TServiceClient : class
        {
            serviceCollection.AddTransient<TServiceClient>();
            return serviceCollection;
        }

        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var url = configuration.GetValue<string>("AppSettings:ConsulUrl");
                consulConfig.Address = new Uri(url);
            }));

            return services;
        }

        /// <summary>
        /// Consul注册
        /// </summary>
        private static async Task<WriteResult> RegisterConsul<TServiceImp>(this Server server, IServiceCollection serviceCollection) where TServiceImp : class
        {
            var client = serviceCollection.GetService<IConsulClient>();
            if (client == null)
                throw new Exception("Please register ConsulClient before AddGrpcServer()");

            var serverIP = GetLocalIP();
            var serverPort = serviceCollection.GetService<IConfiguration>().GetValue<int>("AppSettings:Port");
            var registerID = $"{typeof(TServiceImp).Name}({serverPort})";
            await client.Agent.ServiceDeregister(registerID);
            var result = await client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = registerID,
                Name = typeof(TServiceImp).Name,
                Address = serverIP,
                Port = serverPort,
                Check = new AgentServiceCheck
                {
                    TCP = $"{serverIP}:{serverPort}",
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    Interval = TimeSpan.FromSeconds(10),
                    Timeout = TimeSpan.FromSeconds(5)
                }
            });

            return result;
        }

        private static string GetLocalIP()
        {
            var hostName = Dns.GetHostName();
            var ipEntry = Dns.GetHostEntry(hostName);
            for (int i = 0; i < ipEntry.AddressList.Length; i++)
            {
                if (ipEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    return ipEntry.AddressList[i].ToString();
                }
            }

            return "127.0.0.1";
        }

        public static void UseGrpcServer(this IApplicationBuilder app)
        {
            var server = app.ApplicationServices.GetService<Server>();
            server.Start();
            server.Ports.ToList().ForEach(a => Console.WriteLine($"GrpcServer now is listening on  http://localhost:{a.Port}..."));
        }
    }
}
