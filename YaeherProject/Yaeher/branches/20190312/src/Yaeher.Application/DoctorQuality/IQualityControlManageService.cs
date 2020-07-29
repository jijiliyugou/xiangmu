using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.DoctorQuality.Dto;

namespace Yaeher.DoctorQuality
{
    /// <summary>
    /// 处理质控
    /// </summary>
    public interface IQualityControlManageService : IApplicationService
    {
        /// <summary>
        /// 新建处理质控
        /// </summary>
        /// <param name="QualityControlManageInfo"></param>
        /// <returns></returns>
        Task<QualityControlManage> CreateQualityControlManage(QualityControlManage QualityControlManageInfo);
        /// <summary>
        /// 删除处理质控
        /// </summary>
        /// <param name="QualityControlManageInfo"></param>
        /// <returns></returns>
        Task<QualityControlManage> DeleteQualityControlManage(QualityControlManage QualityControlManageInfo);
        /// <summary>
        /// 查询处理质控byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<QualityControlManage> QualityControlManageByID(int Id);
        /// <summary>
        /// 查询处理质控byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<QualityControlManage> QualityControlManageByConsultStateID(int Id);
        /// <summary>
        /// 查询处理质控 List
        /// </summary>
        /// <param name="QualityControlManageInfo"></param>
        /// <returns></returns>
        Task<IList<QualityControlManage>> QualityControlManageList(QualityControlManageIn QualityControlManageInfo);
        /// <summary>
        /// 查询处理质控 page
        /// </summary>
        /// <param name="QualityControlManageInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<QualityControlManagePage>> QualityControlManagePage(QualityControlManageIn QualityControlManageInfo);
        /// <summary>
        /// 修改处理质控
        /// </summary>
        /// <param name="QualityControlManageInfo"></param>
        /// <returns></returns>
        Task<QualityControlManage> UpdateQualityControlManage(QualityControlManage QualityControlManageInfo);
    }
}