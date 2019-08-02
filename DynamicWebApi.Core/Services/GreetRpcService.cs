using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using DynamicWebApi.Core.Extends;
using DynamicWebApi.Core.Services.Rpc.Greet;

namespace DynamicWebApi.Core.Services
{
    /// <summary>
    /// GreetService
    /// </summary>
    [GrpcServiceBind(BindType = typeof(IGreetRpcService), ClientType = typeof(IGreetRpcService.IGreetRpcServiceClient))]
    public class GreetRpcService : IGreetRpcService.IGreetRpcServiceBase
    {
        /// <summary>
        /// SayHello
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
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
