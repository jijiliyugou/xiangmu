using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.YaeherDoctors
{
    /// <summary>
    /// 医生申请门诊
    /// </summary>
    public interface IDoctorClinicApplyService : IApplicationService
    {
        /// <summary>
        /// 新建医生申请门诊
        /// </summary>
        /// <param name="DoctorClinicApplyInfo"></param>
        /// <returns></returns>
        Task<DoctorClinicApply> CreateDoctorClinicApply(DoctorClinicApply DoctorClinicApplyInfo);
        /// <summary>
        /// 删除医生申请门诊
        /// </summary>
        /// <param name="DoctorClinicApplyInfo"></param>
        /// <returns></returns>
        Task<DoctorClinicApply> DeleteDoctorClinicApply(DoctorClinicApply DoctorClinicApplyInfo);
        /// <summary>
        /// 医生申请门诊byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DoctorClinicApply> DoctorClinicApplyByID(int Id);
        /// <summary>
        /// 医生申请门诊 List
        /// </summary>
        /// <param name="DoctorClinicApplyInfo"></param>
        /// <returns></returns>
        Task<List<DoctorClinicApply>> DoctorClinicApplyList(DoctorClinicApplyIn DoctorClinicApplyInfo);
       
        /// <summary>
        /// 医生个人中心门诊 page
        /// </summary>
        /// <param name="DoctorClinicApplyInfo"></param>
        /// <returns></returns>
        Task<List<DoctorClinicApplyOutDetail>> DoctorClinicApplyOutDetailList(DoctorClinicApplyIn DoctorClinicApplyInfo);
        /// <summary>
        /// 医生个人中心门诊 page
        /// </summary>
        /// <param name="DoctorClinicApplyInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<DoctorClinicApplyOutDetail>> DoctorClinicApplyOutDetailPage(DoctorClinicApplyIn DoctorClinicApplyInfo);
        /// <summary>
        /// 修改医生申请门诊
        /// </summary>
        /// <param name="DoctorClinicApplyInfo"></param>
        /// <returns></returns>
        Task<DoctorClinicApply> UpdateDoctorClinicApply(DoctorClinicApply DoctorClinicApplyInfo);
    }
}