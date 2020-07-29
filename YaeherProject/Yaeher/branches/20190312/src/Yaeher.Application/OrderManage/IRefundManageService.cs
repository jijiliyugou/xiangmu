using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Yaeher
{
    /// <summary>
    /// 订单退单管理
    /// </summary>
    public interface IRefundManageService : IApplicationService
    {
        /// <summary>
        /// 新建订单退单管理
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        Task<RefundManage> CreateRefundManage(RefundManage RefundManageInfo);
        /// <summary>
        /// 删除订单退单管理
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        Task<RefundManage> DeleteRefundManage(RefundManage RefundManageInfo);
        /// <summary>
        /// 查询订单退单管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<RefundManage> RefundManageByID(int Id);
        /// <summary>
        /// 查询订单退单管理bynumber
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<RefundManage> RefundManageByNumber(string number);
        /// <summary>
        /// 查询订单退单管理by咨询单ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<RefundManage> RefundManageByConsulID(int Id);
        /// <summary>
        /// 查询订单退单管理 List
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        Task<IList<RefundManage>> RefundManageList(RefundManageIn RefundManageInfo);
        /// <summary>
        /// 查询订单退单管理 List
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        Task<List<RefundManage>> PayCheckRefundManageList(RefundManageIn RefundManageInfo);
        /// <summary>
        /// 查询订单退单管理 page
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<RefundManage>> RefundManagePage(RefundManageIn RefundManageInfo);
       
      
        /// <summary>
        /// 修改订单退单管理
        /// </summary>
        /// <param name="RefundManageInfo"></param>
        /// <returns></returns>
        Task<RefundManage> UpdateRefundManage(RefundManage RefundManageInfo);
    }
}