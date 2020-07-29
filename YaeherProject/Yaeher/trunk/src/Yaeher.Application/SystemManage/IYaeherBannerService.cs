using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemConfig;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    public interface IYaeherBannerService: IApplicationService
    {
        Task<YaeherBanner> CreateYaeherBanner(YaeherBanner YaeherBannerInfo);
        Task<YaeherBanner> DeleteYaeherBanner(YaeherBanner YaeherBannerInfo);
        Task<YaeherBanner> UpdateYaeherBanner(YaeherBanner YaeherBannerInfo);
        Task<YaeherBanner> YaeherBannerByID(int Id);
        Task<IList<YaeherBanner>> YaeherBannerList(YaeherBannerIn YaeherBannerInfo);
        Task<PagedResultDto<YaeherBanner>> YaeherBannerPage(YaeherBannerIn YaeherBannerInfo);
    }
}