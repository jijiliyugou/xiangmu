using System.Threading.Tasks;
using Yaeher.Configuration.Dto;

namespace Yaeher.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
