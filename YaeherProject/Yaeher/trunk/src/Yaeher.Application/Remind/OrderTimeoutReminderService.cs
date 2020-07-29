using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.Remind.Dto;

namespace Yaeher.Remind
{
    /// <summary>
    /// 超时提醒
    /// </summary>
    public class OrderTimeoutReminderService : IOrderTimeoutReminderService
    {
        private readonly IRepository<OrderTimeoutReminder> _repository;
        /// <summary>
        /// 超时提醒 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public OrderTimeoutReminderService(IRepository<OrderTimeoutReminder> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询超时提醒 List
        /// </summary>
        /// <param name="OrderTimeoutReminderInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<OrderTimeoutReminder>> OrderTimeoutReminderList(OrderTimeoutReminderIn OrderTimeoutReminderInfo)
        {
            var OrderTimeoutReminders = await _repository.GetAllListAsync(OrderTimeoutReminderInfo.Expression);
            return OrderTimeoutReminders.ToList();
        }

        /// <summary>
        /// 查询超时提醒byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderTimeoutReminder> OrderTimeoutReminderByID(int Id)
        {
            var OrderTimeoutReminders = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return OrderTimeoutReminders;
        }
        /// <summary>
        /// 查询超时提醒 page
        /// </summary>
        /// <param name="OrderTimeoutReminderInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<OrderTimeoutReminder>> OrderTimeoutReminderPage(OrderTimeoutReminderIn OrderTimeoutReminderInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(OrderTimeoutReminderInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / OrderTimeoutReminderInfo.MaxResultCount;
            var OrderTimeoutReminderList = await query.PageBy(OrderTimeoutReminderInfo.SkipTotal, OrderTimeoutReminderInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<OrderTimeoutReminder>(tasksCount, OrderTimeoutReminderList.MapTo<List<OrderTimeoutReminder>>());
        }
        /// <summary>
        /// 新建超时提醒
        /// </summary>
        /// <param name="OrderTimeoutReminderInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderTimeoutReminder> CreateOrderTimeoutReminder(OrderTimeoutReminder OrderTimeoutReminderInfo)
        {
            OrderTimeoutReminderInfo.Id= await _repository.InsertAndGetIdAsync(OrderTimeoutReminderInfo);
            return OrderTimeoutReminderInfo;
        }

        /// <summary>
        /// 修改超时提醒
        /// </summary>
        /// <param name="OrderTimeoutReminderInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderTimeoutReminder> UpdateOrderTimeoutReminder(OrderTimeoutReminder OrderTimeoutReminderInfo)
        {
            return await _repository.UpdateAsync(OrderTimeoutReminderInfo);
        }

        /// <summary>
        /// 删除超时提醒
        /// </summary>
        /// <param name="OrderTimeoutReminderInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderTimeoutReminder> DeleteOrderTimeoutReminder(OrderTimeoutReminder OrderTimeoutReminderInfo)
        {
            return await _repository.UpdateAsync(OrderTimeoutReminderInfo);
        }
    }
}
