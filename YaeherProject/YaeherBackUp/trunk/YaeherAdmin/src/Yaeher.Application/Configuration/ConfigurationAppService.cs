using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Yaeher.Configuration.Dto;

namespace Yaeher.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : YaeherAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
