using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.Scheduling.Dto;

namespace Yaeher.Scheduling
{
    /// <summary>
    /// 医生排班
    /// </summary>
    public interface IDoctorSchedulingService : IApplicationService
    {
        /// <summary>
        /// 新建医生排班
        /// </summary>
        /// <param name="DoctorSchedulingInfo"></param>
        /// <returns></returns>
        Task<DoctorScheduling> CreateDoctorScheduling(DoctorScheduling DoctorSchedulingInfo);

        /// <summary>
        /// 删除医生排班
        /// </summary>
        /// <param name="DoctorSchedulingInfo"></param>
        /// <returns></returns>
        Task<DoctorScheduling> DeleteDoctorScheduling(DoctorScheduling DoctorSchedulingInfo);
        /// <summary>
        /// 查询医生排班byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DoctorScheduling> DoctorSchedulingByID(int Id);
        /// <summary>
        /// 查询医生排班 List
        /// </summary>
        /// <param name="DoctorSchedulingInfo"></param>
        /// <returns></returns>
        Task<IList<DoctorScheduling>> DoctorSchedulingList(DoctorSchedulingIn DoctorSchedulingInfo);
        /// <summary>
        /// 查询医生排班 page
        /// </summary>
        /// <param name="DoctorSchedulingInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<DoctorScheduling>> DoctorSchedulingPage(DoctorSchedulingIn DoctorSchedulingInfo);
        /// <summary>
        /// 修改医生排班
        /// </summary>
        /// <param name="DoctorSchedulingInfo"></param>
        /// <returns></returns>
        Task<DoctorScheduling> UpdateDoctorScheduling(DoctorScheduling DoctorSchedulingInfo);
    }
}