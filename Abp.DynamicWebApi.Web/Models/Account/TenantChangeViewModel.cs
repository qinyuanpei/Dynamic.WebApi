using Abp.AutoMapper;
using Abp.DynamicWebApi.Sessions.Dto;

namespace Abp.DynamicWebApi.Web.Models.Account
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}