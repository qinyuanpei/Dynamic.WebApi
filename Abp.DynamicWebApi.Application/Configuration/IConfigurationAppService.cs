using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.DynamicWebApi.Configuration.Dto;

namespace Abp.DynamicWebApi.Configuration
{
    public interface IConfigurationAppService: IApplicationService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}