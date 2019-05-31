using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.DynamicWebApi.Authorization.Users;
using Abp.DynamicWebApi.Users;

namespace Abp.DynamicWebApi.Sessions.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }
    }
}
