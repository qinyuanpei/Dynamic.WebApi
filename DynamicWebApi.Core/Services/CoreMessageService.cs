using DynamicWebApi.Core.Extends;
using GreetGrpc;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicWebApi.Core.Services
{
    public class CoreMessageService : IDynamicController
    {
        private GreeterGrpcService.GreeterGrpcServiceClient _client;
        public CoreMessageService(GreeterGrpcService.GreeterGrpcServiceClient client)
        {
            _client = client;
        }

        [HttpGet]
        public string Echo(string receiver)
        {
            return $"Hello, {receiver}";
        }

        [HttpGet]
        public string Greet()
        {
            var reply = _client.SayHello(new HelloRequest { Name = "PayneQin .NET Core Client" });
            return reply.Message;
        }
    }
}
