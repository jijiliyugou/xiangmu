using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 医生审核
    /// </summary>
    public interface IDoctorCheckService : IApplicationService
    {
        /// <summary>
        /// 新建医生审核
        /// </summary>
        /// <param name="DoctorCheckInfo"></param>
        /// <returns></returns>
        Task<DoctorCheck> CreateDoctorCheck(DoctorCheck DoctorCheckInfo);
        /// <summary>
        /// 删除医生审核
        /// </summary>
        /// <param name="DoctorCheckInfo"></param>
        /// <returns></returns>
        Task<DoctorCheck> DeleteDoctorCheck(DoctorCheck DoctorCheckInfo);
        /// <summary>
        /// 查询医生审核byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DoctorCheck> DoctorCheckByID(int Id);
        /// <summary>
        /// 查询医生审核 List
        /// </summary>
        /// <param name="DoctorCheckInfo"></param>
        /// <returns></returns>
        Task<IList<DoctorCheck>> DoctorCheckList(DoctorCheckIn DoctorCheckInfo);
        /// <summary>
        /// 查询医生审核 page
        /// </summary>
        /// <param name="DoctorCheckInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<DoctorCheck>> DoctorCheckPage(DoctorCheckIn DoctorCheckInfo);
        /// <summary>
        /// 修改医生审核
        /// </summary>
        /// <param name="DoctorCheckInfo"></param>
        /// <returns></returns>
        Task<DoctorCheck> UpdateDoctorCheck(DoctorCheck DoctorCheckInfo);
    }
}