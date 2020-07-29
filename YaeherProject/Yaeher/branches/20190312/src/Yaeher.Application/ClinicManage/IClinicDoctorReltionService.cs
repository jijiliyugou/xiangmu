using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.ClinicManage.Dto;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.ClinicManage
{
    /// <summary>
    /// 门诊与医生关系
    /// </summary>
    public interface IClinicDoctorReltionService : IApplicationService
    {
        /// <summary>
        /// 查询门诊与医生关系byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ClinicDoctorReltion> ClinicDoctorReltionByID(int Id);
        /// <summary>
        /// 查询门诊与医生关系 List
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        Task<IList<ClinicDoctorReltion>> ClinicDoctorReltionList(ClinicDoctorReltionIn ClinicDoctorReltionInfo);

        /// <summary>
        /// 查医生所有门诊 List
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        Task<List<DoctorClinicApplyOutDetail>> ClinicDoctorReltionApplyList(ClinicDoctorReltionIn ClinicDoctorReltionInfo);
        
        /// <summary>
        /// 查询门诊与医生关系 page
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<ClinicDoctorReltion>> ClinicDoctorReltionPage(ClinicDoctorReltionIn ClinicDoctorReltionInfo);
        /// <summary>
        /// 新建门诊与医生关系
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        Task<ClinicDoctorReltion> CreateClinicDoctorReltion(ClinicDoctorReltion ClinicDoctorReltionInfo);
        /// <summary>
        /// 删除门诊与医生关系
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        Task<ClinicDoctorReltion> DeleteClinicDoctorReltion(ClinicDoctorReltion ClinicDoctorReltionInfo);
        /// <summary>
        /// 修改门诊与医生关系
        /// </summary>
        /// <param name="ClinicDoctorReltionInfo"></param>
        /// <returns></returns>
        Task<ClinicDoctorReltion> UpdateClinicDoctorReltion(ClinicDoctorReltion ClinicDoctorReltionInfo);
    }
}