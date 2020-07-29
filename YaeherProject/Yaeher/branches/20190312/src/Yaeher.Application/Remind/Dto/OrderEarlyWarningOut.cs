using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Yaeher.Remind.Dto
{
    /// <summary>
    /// 订单预警
    /// </summary>
    public class OrderEarlyWarningOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="OrderEarlyWarningDto"></param>
        /// <param name="OrderEarlyWarningInfo"></param>
        public OrderEarlyWarningOut(PagedResultDto<OrderEarlyWarning> OrderEarlyWarningDto, OrderEarlyWarningIn OrderEarlyWarningInfo)
        {
            Items = OrderEarlyWarningDto.Items;
            TotalCount = OrderEarlyWarningDto.TotalCount;
            TotalPage = OrderEarlyWarningDto.TotalCount / OrderEarlyWarningInfo.MaxResultCount;
            SkipCount = OrderEarlyWarningInfo.SkipCount;
            MaxResultCount = OrderEarlyWarningInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<OrderEarlyWarning> Items { get; set; }
    }
}
