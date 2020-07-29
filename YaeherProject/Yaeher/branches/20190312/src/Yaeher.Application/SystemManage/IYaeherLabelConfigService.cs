using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    public interface IYaeherLabelConfigService: IApplicationService
    {
        Task<YaeherLabelConfig> CreateYaeherLabelConfig(YaeherLabelConfig YaeherLabelConfigInfo);
        Task<YaeherLabelConfig> DeleteYaeherLabelConfig(YaeherLabelConfig YaeherLabelConfigInfo);
        Task<YaeherLabelConfig> UpdateYaeherLabelConfig(YaeherLabelConfig YaeherLabelConfigInfo);
        Task<YaeherLabelConfig> YaeherLabelConfigByID(int Id);
        Task<IList<YaeherLabelConfig>> YaeherLabelConfigList(YaeherLabelConfigIn YaeherLabelConfigInfo);
        Task<IList<YaeherLabelList>> YaeherModuleList(YaeherLabelConfigIn YaeherLabelConfigInfo);
        Task<PagedResultDto<YaeherLabelConfig>> YaeherLabelConfigPage(YaeherLabelConfigIn YaeherLabelConfigInfo);
    }
}