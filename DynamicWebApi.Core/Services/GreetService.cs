using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using DynamicWebApi.Core.Extends;
using GreetGrpc;

namespace DynamicWebApi.Core.Services
{
    [GrpcServiceBind(typeof(GreetGrpc.GreeterGrpcService))]
    public class GreeterService : GreetGrpc.GreeterGrpcService.GreeterGrpcServiceBase
    {
        public override Task<HelloReply> SayHello(
            HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
