using System.Threading.Tasks;
using Abp.Auditing;
using Abp.AutoMapper;
using Abp.DynamicWebApi.Sessions.Dto;

namespace Abp.DynamicWebApi.Sessions
{
    public class SessionAppService : DynamicWebApiAppServiceBase, ISessionAppService
    {
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput();

            if (AbpSession.UserId.HasValue)
            {
                output.User = (await GetCurrentUserAsync()).MapTo<UserLoginInfoDto>();
            }

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = (await GetCurrentTenantAsync()).MapTo<TenantLoginInfoDto>();
            }

            return output;
        }
    }
}