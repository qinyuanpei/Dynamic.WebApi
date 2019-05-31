using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.DynamicWebApi.MultiTenancy.Dto;

namespace Abp.DynamicWebApi.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
