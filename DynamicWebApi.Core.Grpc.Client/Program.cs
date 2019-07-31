using GreetGrpc;
using Grpc.Core;
using System;

namespace DynamicWebApi.Core.Grpc.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var channel = new Channel("127.0.0.1:5000", ChannelCredentials.Insecure);
            var client = new GreetGrpc.GreeterGrpcService.GreeterGrpcServiceClient(channel);
            var reply = client.SayHello(new HelloRequest { Name = "PayneQin .NET Core Client" });
            Console.WriteLine("来自" + reply.Message);

            channel.ShutdownAsync().Wait();
            Console.WriteLine("任意键退出...");
            Console.ReadKey();
        }
    }
}
