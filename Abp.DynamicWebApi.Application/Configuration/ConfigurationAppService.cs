using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Abp.DynamicWebApi.Configuration.Dto;

namespace Abp.DynamicWebApi.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : DynamicWebApiAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
