using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace Yaeher.Remind.Dto
{
    /// <summary>
    /// 超时提醒
    /// </summary>
    public class OrderTimeoutReminderOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="OrderTimeoutReminderDto"></param>
        /// <param name="OrderTimeoutReminderInfo"></param>
        public OrderTimeoutReminderOut(PagedResultDto<OrderTimeoutReminder> OrderTimeoutReminderDto, OrderTimeoutReminderIn OrderTimeoutReminderInfo)
        {
            Items = OrderTimeoutReminderDto.Items;
            TotalCount = OrderTimeoutReminderDto.TotalCount;
            TotalPage = OrderTimeoutReminderDto.TotalCount / OrderTimeoutReminderInfo.MaxResultCount;
            SkipCount = OrderTimeoutReminderInfo.SkipCount;
            MaxResultCount = OrderTimeoutReminderInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<OrderTimeoutReminder> Items { get; set; }
    }
}
