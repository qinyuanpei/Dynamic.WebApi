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
        private UserGrpc.UserGrpcService.UserGrpcServiceClient _client;
        public UserInfoController(UserGrpc.UserGrpcService.UserGrpcServiceClient client)
        {
            _client = client;
        }

        [HttpGet("api/Users/{id}")]
        public UserGrpc.UserGrpcEdit Get(int id)
        {
            var reply = _client.GetUser(new UserGrpc.UserGrpcQuery() { Uid = id });
            return reply;
        }

        [HttpPost("api/Users/")]
        public async Task<UserGrpc.RpcResponse> Post(UserGrpc.UserGrpcEdit userInfo)
        {
            var reply = await _client.SaveUserAsync(userInfo);
            return reply;
        }
    }
}
