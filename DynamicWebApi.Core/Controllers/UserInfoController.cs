using DynamicWebApi.Core.Extends;
using DynamicWebApi.Core.Services.Rpc.User;
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
        private IUserRpcService.IUserRpcServiceClient _client;
        public UserInfoController(IUserRpcService.IUserRpcServiceClient client)
        {
            _client = client;
        }

        [HttpGet("api/Users/{id}")]
        public UserGrpcEdit Get(int id)
        {
            var reply = _client.GetUser(new UserGrpcQuery() { Uid = id });
            return reply;
        }

        [HttpPost("api/Users/")]
        public async Task<RpcResponse> Post(UserGrpcEdit userInfo)
        {
            var reply = await _client.SaveUserAsync(userInfo);
            return reply;
        }
    }
}
