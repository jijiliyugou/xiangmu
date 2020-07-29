using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Yaeher
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public interface IOrderManageService : IApplicationService
    {
        /// <summary>
        /// 新建订单管理
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        Task<OrderManage> CreateOrderManage(OrderManage OrderManageInfo);
        /// <summary>
        /// 删除订单管理
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        Task<OrderManage> DeleteOrderManage(OrderManage OrderManageInfo);
        /// <summary>
        /// 查询订单管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<OrderManage> OrderManageByID(int Id);
        /// <summary>
        /// 查询订单管理byConsultationID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<OrderManage> OrderManageByConsultationID(int Id);
        /// <summary>
        /// 查询订单管理byconsultNumber
        /// </summary>
        /// <param name="consultNumber"></param>
        /// <returns></returns>
        Task<OrderManage> OrderManageByconsultNumber(string consultNumber);
        /// <summary>
        /// 查询订单管理 List
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        Task<IList<OrderManage>> OrderManageList(OrderManageIn OrderManageInfo);
        /// <summary>
        /// 查询订单管理 page
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<OrderManage>> OrderManagePage(OrderManageIn OrderManageInfo);
        /// <summary>
        /// 查询订单管理 page
        /// </summary>
        /// <param name="consultationIn"></param>
        /// <returns></returns>
        Task<PagedResultDto<OrderManageDetail>> TotalOrderManagePage(ConsultationIn consultationIn);
        /// <summary>
        /// 修改订单管理
        /// </summary>
        /// <param name="OrderManageInfo"></param>
        /// <returns></returns>
        Task<OrderManage> UpdateOrderManage(OrderManage OrderManageInfo);
    }
}