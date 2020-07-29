using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    public interface ISystemConfigsService: IApplicationService
    {
        Task<SystemConfigs> CreateSystemConfigs(SystemConfigs SystemConfigsInfo);
        Task<SystemConfigs> SystemConfigsByID(int Id);
        Task<List<SystemConfigs>> SystemConfigsList(SystemConfigsIn SystemConfigsInfo);
        Task<PagedResultDto<SystemConfigs>> SystemConfigsPage(SystemConfigsIn SystemConfigsInfo);
        Task<SystemConfigs> UpdateSystemConfigs(SystemConfigs SystemConfigsInfo);
    }
}