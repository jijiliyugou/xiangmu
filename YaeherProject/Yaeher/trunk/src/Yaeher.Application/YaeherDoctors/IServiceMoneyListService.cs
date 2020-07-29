using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.YaeherDoctors
{
    /// <summary>
    /// 医生费用表
    /// </summary>
    public interface IServiceMoneyListService: IApplicationService
    {
        /// <summary>
        /// 新建医生费用表
        /// </summary>
        /// <param name="ServiceMoneyListInfo"></param>
        /// <returns></returns>
        Task<ServiceMoneyList> CreateServiceMoneyList(ServiceMoneyList ServiceMoneyListInfo);
        /// <summary>
        /// 删除医生费用表
        /// </summary>
        /// <param name="ServiceMoneyListInfo"></param>
        /// <returns></returns>
        Task<ServiceMoneyList> DeleteServiceMoneyList(ServiceMoneyList ServiceMoneyListInfo);
        /// <summary>
        /// 查询医生费用表byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ServiceMoneyList> ServiceMoneyListByID(int Id);
        /// <summary>
        /// 查询医生费用表 List
        /// </summary>
        /// <param name="ServiceMoneyListInfo"></param>
        /// <returns></returns>
        Task<IList<ServiceMoneyList>> ServiceMoneyListList(ServiceMoneyListIn ServiceMoneyListInfo);
        /// <summary>
        /// 查询医生费用表 List
        /// </summary>
        /// <param name="ServiceMoneyListInfo"></param>
        /// <returns></returns>
        Task<IList<ServiceMoneyStateList>> ServiceMoneyStateList(ServiceMoneyListIn ServiceMoneyListInfo);
        
        /// <summary>
        /// 查询医生费用表 page
        /// </summary>
        /// <param name="ServiceMoneyListInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<ServiceMoneyList>> ServiceMoneyListPage(ServiceMoneyListIn ServiceMoneyListInfo);
       
        /// <summary>
        /// 修改医生费用表
        /// </summary>
        /// <param name="ServiceMoneyListInfo"></param>
        /// <returns></returns>
        Task<ServiceMoneyList> UpdateServiceMoneyList(ServiceMoneyList ServiceMoneyListInfo);
    }
}