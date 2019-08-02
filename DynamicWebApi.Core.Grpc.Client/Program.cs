using GreetGrpc;
using Grpc.Core;
using System;

namespace DynamicWebApi.Core.Grpc.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var channel = new Channel("localhost:2345", ChannelCredentials.Insecure);
            var client = new UserGrpc.UserGrpcService.UserGrpcServiceClient(channel);
            var reply = client.GetUser(new UserGrpc.UserGrpcQuery() { Uid = 1 });
            Console.WriteLine("来自" + reply.Name);

            channel.ShutdownAsync().Wait();
            Console.WriteLine("任意键退出...");
            Console.ReadKey();
        }
    }
}
