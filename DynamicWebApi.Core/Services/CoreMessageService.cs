using DynamicWebApi.Core.Extends;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicWebApi.Core.Services.Rpc.Greet;

namespace DynamicWebApi.Core.Services
{
    public class CoreMessageService : IDynamicController
    {
        private IGreetRpcService.IGreetRpcServiceClient _client;

        /// <summary>
        /// CoreMessage Service
        /// </summary>
        /// <param name="client"></param>
        public CoreMessageService(IGreetRpcService.IGreetRpcServiceClient client)
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
