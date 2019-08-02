using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using UserGrpc;
using DynamicWebApi.Core.Extends;

namespace DynamicWebApi.Core.Services
{
    [GrpcServiceBind(typeof(UserGrpc.UserGrpcService))]
    public class UserService : UserGrpc.UserGrpcService.UserGrpcServiceBase
    {
        private List<UserGrpcEdit> _users => GetMockUsers();

        public override Task<UserGrpcEdit> GetUser(UserGrpcQuery request, ServerCallContext context)
        {
            var userInfo = _users.FirstOrDefault(u => u.Uid == request.Uid);
            return Task.FromResult(userInfo);
        }

        public override Task<RpcResponse> SaveUser(UserGrpcEdit request, ServerCallContext context)
        {
            var userInfo = _users.FirstOrDefault(u => u.Uid == request.Uid);
            if (userInfo != null)
                return Task.FromResult(new RpcResponse() { Code = "500", Message = $"当前用户:{userInfo.Uid}已存在，不允许重复保存" });

            _users.Add(request);
            return Task.FromResult(new RpcResponse() { Code = "200", Message = "保存成功!" });
        }

        private List<UserGrpcEdit> GetMockUsers()
        {
            return new List<UserGrpcEdit>()
            {
                new UserGrpcEdit(){Uid = 1,Name = "李逍遥",Gender=1,Emial="lixiaoyao@xianjian.com",Mobile="1234567890"},
                new UserGrpcEdit(){Uid = 2,Name = "赵灵儿",Gender=2,Emial="zhaoliner@xianjian.com",Mobile="1234567890"},
                new UserGrpcEdit(){Uid = 3,Name = "林月如",Gender=2,Emial="linyueru@xianjian.com",Mobile="1234567890"},
                new UserGrpcEdit(){Uid = 4,Name = "云天河",Gender=1,Emial="yuntianheo@xianjian.com",Mobile="1234567890"},
                new UserGrpcEdit(){Uid = 4,Name = "韩菱纱",Gender=2,Emial="hanlinsha@xianjian.com",Mobile="1234567890"},
            };
        }
    }
}
