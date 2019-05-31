using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.DynamicWebApi.Sessions.Dto;

namespace Abp.DynamicWebApi.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
