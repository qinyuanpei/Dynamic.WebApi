using Grpc.Core;
using System;
using DynamicWebApi.Core.Client.Services.Rpc.Greet;

namespace DynamicWebApi.Core.Grpc.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var channel = new Channel("localhost:2345", ChannelCredentials.Insecure);
            var client = new IGreetRpcService.IGreetRpcServiceClient(channel);
            var reply = client.SayHello(new HelloRequest() { Name = "Payne" });
            Console.WriteLine("收到回复：" + reply.Message);

            channel.ShutdownAsync().Wait();
            Console.WriteLine("任意键退出...");
            Console.ReadKey();
        }
    }
}
