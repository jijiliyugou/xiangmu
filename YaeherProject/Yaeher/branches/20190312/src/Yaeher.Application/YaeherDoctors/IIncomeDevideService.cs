using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.YaeherDoctors
{
    /// <summary>
    /// 订单收入分配_医生
    /// </summary>
    public interface IIncomeDevideService: IApplicationService
    {
        /// <summary>
        /// 新建订单收入分配_医生
        /// </summary>
        /// <param name="IncomeDevide"></param>
        /// <returns></returns>
        Task<IncomeDevide> CreateIncomeDevide(IncomeDevide IncomeDevide);
        /// <summary>
        /// 删除 订单收入分配_医生
        /// </summary>
        /// <param name="IncomeDevide"></param>
        /// <returns></returns>
        Task<IncomeDevide> DeleteIncomeDevide(IncomeDevide IncomeDevide);
        /// <summary>
        /// 查找 订单收入分配_医生ById
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<IncomeDevide> IncomeDevideByID(int Id);
        /// <summary>
        /// 查找 订单收入分配_医生 List
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        Task<IList<IncomeDevide>> IncomeDevideList(IncomeDevideIn IncomeDevideInfo);
        /// <summary>
        /// 查找 订单收入分配_医生 List
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        Task<IList<IncomeDevide>> IncomeForeignDevideList(IncomeDevideIn IncomeDevideInfo);
        /// <summary>
        /// 查找 订单收入分配_医生 List
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        Task<IncomeTotalModel> IncomeTotalModelList(IncomeDevideIn IncomeDevideInfo);
        
        /// <summary>
        /// 查找 订单收入分配_医生 Page
        /// </summary>
        /// <param name="IncomeDevideInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<IncomeDevide>> IncomeDevidePage(IncomeDevideIn IncomeDevideInfo);
        /// <summary>
        /// 更新 订单收入分配_医生
        /// </summary>
        /// <param name="IncomeDevide"></param>
        /// <returns></returns>
        Task<IncomeDevide> UpdateIncomeDevide(IncomeDevide IncomeDevide);
    }
}