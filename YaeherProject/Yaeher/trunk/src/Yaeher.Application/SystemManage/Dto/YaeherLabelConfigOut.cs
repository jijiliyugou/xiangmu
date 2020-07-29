using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// YaeherLabelConfig 输出
    /// </summary>
    public class YaeherLabelConfigOut: PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherLabelConfigDto"></param>
        /// <param name="YaeherLabelConfigInfo"></param>
        public YaeherLabelConfigOut(PagedResultDto<YaeherLabelConfig> YaeherLabelConfigDto, YaeherLabelConfigIn YaeherLabelConfigInfo)
        {
            Items = YaeherLabelConfigDto.Items;
            TotalCount = YaeherLabelConfigDto.TotalCount;
            TotalPage = YaeherLabelConfigDto.TotalCount / YaeherLabelConfigInfo.MaxResultCount;
            SkipCount = YaeherLabelConfigInfo.SkipCount;
            MaxResultCount = YaeherLabelConfigInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherLabelConfig> Items { get; set; }
    }
}
