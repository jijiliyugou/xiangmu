using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Yaeher.Release
{
    /// <summary>
    /// 文章
    /// </summary>
    public class ReleaseManageOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="ReleaseManageDto"></param>
        /// <param name="ReleaseManageInfo"></param>
        public ReleaseManageOut(PagedResultDto<ReleaseManage> ReleaseManageDto, ReleaseManageIn ReleaseManageInfo)
        {
            Items = ReleaseManageDto.Items;
            TotalCount = ReleaseManageDto.TotalCount;
            TotalPage = ReleaseManageDto.TotalCount / ReleaseManageInfo.MaxResultCount;
            SkipCount = ReleaseManageInfo.SkipCount;
            MaxResultCount = ReleaseManageInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<ReleaseManage> Items { get; set; }
    }
}
