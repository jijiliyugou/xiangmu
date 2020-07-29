using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.YaeherDoctors
{
    /// <summary>
    /// 医生服务log日志
    /// </summary>
    public interface IDoctorServiceLogService : IApplicationService
    {
        /// <summary>
        /// 新建医生服务log日志
        /// </summary>
        /// <param name="DoctorServiceLogInfo"></param>
        /// <returns></returns>
        Task<DoctorServiceLog> CreateDoctorServiceLog(DoctorServiceLog DoctorServiceLogInfo);
        /// <summary>
        /// 删除医生服务log日志
        /// </summary>
        /// <param name="DoctorServiceLogInfo"></param>
        /// <returns></returns>
        Task<DoctorServiceLog> DeleteDoctorServiceLog(DoctorServiceLog DoctorServiceLogInfo);
        /// <summary>
        /// 查询医生服务log日志byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DoctorServiceLog> DoctorServiceLogByID(int Id);
        /// <summary>
        /// 查询医生服务log日志 List
        /// </summary>
        /// <param name="DoctorServiceLogInfo"></param>
        /// <returns></returns>
        Task<IList<DoctorServiceLog>> DoctorServiceLogList(DoctorServiceLogIn DoctorServiceLogInfo);
        /// <summary>
        /// 查询医生服务log日志 page
        /// </summary>
        /// <param name="DoctorServiceLogInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<DoctorServiceLog>> DoctorServiceLogPage(DoctorServiceLogIn DoctorServiceLogInfo);
        /// <summary>
        /// 修改医生服务log日志
        /// </summary>
        /// <param name="DoctorServiceLogInfo"></param>
        /// <returns></returns>
        Task<DoctorServiceLog> UpdateDoctorServiceLog(DoctorServiceLog DoctorServiceLogInfo);
    }
}