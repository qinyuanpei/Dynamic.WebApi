using DynamicWebApi.Core.Extends;
using DynamicWebApi.Core.Services;
using DynamicWebApi.Core.Services.Rpc.User;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DynamicWebApi.Core.Services.Rpc.User.IUserRpcService;

namespace DynamicWebApi.Core.Controllers
{
    public class UserInfoController : IDynamicController
    {
        private IServiceDiscover _serviceDiscover;
        public UserInfoController(IServiceDiscover serviceDiscover)
        {
            _serviceDiscover = serviceDiscover;
        }

        [HttpGet("api/Users/{id}")]
        public async Task<UserGrpcEdit> Get(int id)
        {
            var client = (await _serviceDiscover.FindServiceAsync<UserRpcService>()) as IUserRpcServiceClient;
            var reply = client.GetUser(new UserGrpcQuery() { Uid = id });
            return reply;
        }

        [HttpPost("api/Users/")]
        public async Task<RpcResponse> Post(UserGrpcEdit userInfo)
        {
            var client = (await _serviceDiscover.FindServiceAsync<UserRpcService>()) as IUserRpcServiceClient;
            var reply = await client.SaveUserAsync(userInfo);
            return reply;
        }
    }
}
