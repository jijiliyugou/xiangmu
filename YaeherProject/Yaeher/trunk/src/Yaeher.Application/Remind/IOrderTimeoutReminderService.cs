using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.Remind.Dto;

namespace Yaeher.Remind
{
    /// <summary>
    /// 超时提醒
    /// </summary>
    public interface IOrderTimeoutReminderService : IApplicationService
    {
        /// <summary>
        /// 新建超时提醒
        /// </summary>
        /// <param name="OrderTimeoutReminderInfo"></param>
        /// <returns></returns>
        Task<OrderTimeoutReminder> CreateOrderTimeoutReminder(OrderTimeoutReminder OrderTimeoutReminderInfo);
        /// <summary>
        /// 删除超时提醒
        /// </summary>
        /// <param name="OrderTimeoutReminderInfo"></param>
        /// <returns></returns>
        Task<OrderTimeoutReminder> DeleteOrderTimeoutReminder(OrderTimeoutReminder OrderTimeoutReminderInfo);
        /// <summary>
        /// 查询超时提醒byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<OrderTimeoutReminder> OrderTimeoutReminderByID(int Id);
        /// <summary>
        /// 查询超时提醒 List
        /// </summary>
        /// <param name="OrderTimeoutReminderInfo"></param>
        /// <returns></returns>
        Task<IList<OrderTimeoutReminder>> OrderTimeoutReminderList(OrderTimeoutReminderIn OrderTimeoutReminderInfo);
        /// <summary>
        /// 查询超时提醒 page
        /// </summary>
        /// <param name="OrderTimeoutReminderInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<OrderTimeoutReminder>> OrderTimeoutReminderPage(OrderTimeoutReminderIn OrderTimeoutReminderInfo);
        /// <summary>
        /// 修改超时提醒
        /// </summary>
        /// <param name="OrderTimeoutReminderInfo"></param>
        /// <returns></returns>
        Task<OrderTimeoutReminder> UpdateOrderTimeoutReminder(OrderTimeoutReminder OrderTimeoutReminderInfo);
    }
}