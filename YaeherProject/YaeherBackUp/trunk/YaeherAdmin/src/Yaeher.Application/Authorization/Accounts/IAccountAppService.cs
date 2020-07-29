using System.Threading.Tasks;
using Abp.Application.Services;
using Yaeher.Authorization.Accounts.Dto;

namespace Yaeher.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
