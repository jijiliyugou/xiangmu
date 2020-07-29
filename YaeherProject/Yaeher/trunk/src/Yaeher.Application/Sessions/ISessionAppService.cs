using System.Threading.Tasks;
using Abp.Application.Services;
using Yaeher.Sessions.Dto;

namespace Yaeher.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
