using DynamicWebApi.Core.Extends;
using GreetGrpc;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicWebApi.Core.Controllers
{
    public class UserInfoController : IDynamicController
    {
        private Channel _channel;
        public UserInfoController()
        {
            _channel = new Channel("127.0.0.1:5000", ChannelCredentials.Insecure);
        }

        [HttpGet("api/Users/{id}")]
        public UserGrpc.UserGrpcEdit Get(int id)
        {
            var client = new UserGrpc.UserGrpcService.UserGrpcServiceClient(_channel);
            var reply = client.GetUser(new UserGrpc.UserGrpcQuery() { Uid = id });
            return reply;
        }

        [HttpPost("api/Users/")]
        public async Task<UserGrpc.RpcResponse> Post(UserGrpc.UserGrpcEdit userInfo)
        {
            var client = new UserGrpc.UserGrpcService.UserGrpcServiceClient(_channel);
            var reply = await client.SaveUserAsync(userInfo);
            return reply;
        }
    }
}
