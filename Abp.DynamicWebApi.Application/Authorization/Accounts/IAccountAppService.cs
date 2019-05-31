using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.DynamicWebApi.Authorization.Accounts.Dto;

namespace Abp.DynamicWebApi.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
