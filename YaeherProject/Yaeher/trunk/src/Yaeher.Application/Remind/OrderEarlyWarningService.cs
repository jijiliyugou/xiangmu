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
    /// 订单预警
    /// </summary>
    public class OrderEarlyWarningService : IOrderEarlyWarningService
    {
        private readonly IRepository<OrderEarlyWarning> _repository;
        /// <summary>
        /// 订单预警 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public OrderEarlyWarningService(IRepository<OrderEarlyWarning> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 查询订单预警 List
        /// </summary>
        /// <param name="OrderEarlyWarningInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<OrderEarlyWarning>> OrderEarlyWarningList(OrderEarlyWarningIn OrderEarlyWarningInfo)
        {
            var OrderEarlyWarnings = await _repository.GetAllListAsync(OrderEarlyWarningInfo.Expression);
            return OrderEarlyWarnings.ToList();
        }

        /// <summary>
        /// 查询订单预警byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderEarlyWarning> OrderEarlyWarningByID(int Id)
        {
            var OrderEarlyWarnings = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return OrderEarlyWarnings;
        }
        /// <summary>
        /// 查询订单预警 page
        /// </summary>
        /// <param name="OrderEarlyWarningInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<OrderEarlyWarning>> OrderEarlyWarningPage(OrderEarlyWarningIn OrderEarlyWarningInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(OrderEarlyWarningInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / OrderEarlyWarningInfo.MaxResultCount;
            var OrderEarlyWarningList = await query.PageBy(OrderEarlyWarningInfo.SkipTotal, OrderEarlyWarningInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<OrderEarlyWarning>(tasksCount, OrderEarlyWarningList.MapTo<List<OrderEarlyWarning>>());
        }
        /// <summary>
        /// 新建订单预警
        /// </summary>
        /// <param name="OrderEarlyWarningInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderEarlyWarning> CreateOrderEarlyWarning(OrderEarlyWarning OrderEarlyWarningInfo)
        {
            OrderEarlyWarningInfo.Id= await _repository.InsertAndGetIdAsync(OrderEarlyWarningInfo);
            return OrderEarlyWarningInfo;
        }

        /// <summary>
        /// 修改订单预警
        /// </summary>
        /// <param name="OrderEarlyWarningInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderEarlyWarning> UpdateOrderEarlyWarning(OrderEarlyWarning OrderEarlyWarningInfo)
        {
            return await _repository.UpdateAsync(OrderEarlyWarningInfo);
        }

        /// <summary>
        /// 删除订单预警
        /// </summary>
        /// <param name="OrderEarlyWarningInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<OrderEarlyWarning> DeleteOrderEarlyWarning(OrderEarlyWarning OrderEarlyWarningInfo)
        {
            return await _repository.UpdateAsync(OrderEarlyWarningInfo);
        }
    }
}
