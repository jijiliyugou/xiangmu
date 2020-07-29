using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// YaeherBanner 输出
    /// </summary>
    public class YaeherBannerOut: PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherBannerDto"></param>
        /// <param name="YaeherBannerInfo"></param>
        public YaeherBannerOut(PagedResultDto<YaeherBanner> YaeherBannerDto, YaeherBannerIn YaeherBannerInfo)
        {
            Items = YaeherBannerDto.Items;
            TotalCount = YaeherBannerDto.TotalCount;
            TotalPage = YaeherBannerDto.TotalCount / YaeherBannerInfo.MaxResultCount;
            SkipCount = YaeherBannerInfo.SkipCount;
            MaxResultCount = YaeherBannerInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherBanner> Items { get; set; }
    }
}
