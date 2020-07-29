using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.Remind.Dto;

namespace Yaeher.Remind
{
    /// <summary>
    /// 订单预警
    /// </summary>
    public interface IOrderEarlyWarningService : IApplicationService
    {
        /// <summary>
        /// 新建订单预警
        /// </summary>
        /// <param name="OrderEarlyWarningInfo"></param>
        /// <returns></returns>
        Task<OrderEarlyWarning> CreateOrderEarlyWarning(OrderEarlyWarning OrderEarlyWarningInfo);
        /// <summary>
        /// 删除订单预警
        /// </summary>
        /// <param name="OrderEarlyWarningInfo"></param>
        /// <returns></returns>
        Task<OrderEarlyWarning> DeleteOrderEarlyWarning(OrderEarlyWarning OrderEarlyWarningInfo);
        /// <summary>
        /// 查询订单预警byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<OrderEarlyWarning> OrderEarlyWarningByID(int Id);
        /// <summary>
        /// 查询订单预警 List
        /// </summary>
        /// <param name="OrderEarlyWarningInfo"></param>
        /// <returns></returns>
        Task<IList<OrderEarlyWarning>> OrderEarlyWarningList(OrderEarlyWarningIn OrderEarlyWarningInfo);
        /// <summary>
        /// 查询订单预警 page
        /// </summary>
        /// <param name="OrderEarlyWarningInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<OrderEarlyWarning>> OrderEarlyWarningPage(OrderEarlyWarningIn OrderEarlyWarningInfo);
        /// <summary>
        /// 修改订单预警
        /// </summary>
        /// <param name="OrderEarlyWarningInfo"></param>
        /// <returns></returns>
        Task<OrderEarlyWarning> UpdateOrderEarlyWarning(OrderEarlyWarning OrderEarlyWarningInfo);
    }
}