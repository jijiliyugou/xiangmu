using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 排序设置
    /// </summary>
    public class RecommendedOrderOut: PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="RecommendedOrderDto"></param>
        /// <param name="RecommendedOrdeInfo"></param>
        public RecommendedOrderOut(PagedResultDto<RecommendedOrdering> RecommendedOrderDto, RecommendedOrderIn RecommendedOrdeInfo)
        {
            Items = RecommendedOrderDto.Items;
            TotalCount = RecommendedOrderDto.TotalCount;
            TotalPage = RecommendedOrderDto.TotalCount / RecommendedOrdeInfo.MaxResultCount;
            SkipCount = RecommendedOrdeInfo.SkipCount;
            MaxResultCount = RecommendedOrdeInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<RecommendedOrdering> Items { get; set; }
    }
}
