using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.ClinicManage.Dto;

namespace Yaeher.ClinicManage
{
    /// <summary>
    /// 门诊与标签关系
    /// </summary>
    public interface IClinicLableReltionService : IApplicationService
    {
        /// <summary>
        /// 查询门诊与标签关系 byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ClinicLableReltion> ClinicDoctorReltionByID(int Id);
        /// <summary>
        /// 查询门诊与标签关系 List
        /// </summary>
        /// <param name="ClinicLableReltionInfo"></param>
        /// <returns></returns>
        Task<IList<ClinicLableReltion>> ClinicDoctorReltionList(ClinicLableReltionIn ClinicLableReltionInfo);
        /// <summary>
        /// 查询门诊与标签关系 Page
        /// </summary>
        /// <param name="ClinicLableReltionInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<ClinicLableReltion>> ClinicDoctorReltionPage(ClinicLableReltionIn ClinicLableReltionInfo);
        /// <summary>
        /// 新建门诊与标签关系
        /// </summary>
        /// <param name="ClinicLableReltionInfo"></param>
        /// <returns></returns>
        Task<ClinicLableReltion> CreateClinicDoctorReltion(ClinicLableReltion ClinicLableReltionInfo);
        /// <summary>
        /// 删除门诊与标签关系
        /// </summary>
        /// <param name="ClinicLableReltionInfo"></param>
        /// <returns></returns>
        Task<ClinicLableReltion> DeleteClinicDoctorReltion(ClinicLableReltion ClinicLableReltionInfo);
        /// <summary>
        /// 修改门诊与标签关系
        /// </summary>
        /// <param name="ClinicLableReltionInfo"></param>
        /// <returns></returns>
        Task<ClinicLableReltion> UpdateClinicDoctorReltion(ClinicLableReltion ClinicLableReltionInfo);
    }
}