using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 医生上下线设置log
    /// </summary>
    public interface IDoctorOnlineSetLogService : IApplicationService
    {
        /// <summary>
        /// 新建医生上下线设置log
        /// </summary>
        /// <param name="DoctorOnlineSetLogInfo"></param>
        /// <returns></returns>
        Task<DoctorOnlineSetLog> CreateDoctorOnlineSetLog(DoctorOnlineSetLog DoctorOnlineSetLogInfo);
        /// <summary>
        /// 删除医生上下线设置log
        /// </summary>
        /// <param name="DoctorOnlineSetLogInfo"></param>
        /// <returns></returns>
        Task<DoctorOnlineSetLog> DeleteDoctorOnlineSetLog(DoctorOnlineSetLog DoctorOnlineSetLogInfo);
        /// <summary>
        /// 查询医生上下线设置logbyId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DoctorOnlineSetLog> DoctorOnlineSetLogByID(int Id);
        /// <summary>
        /// 查询医生上下线设置log List
        /// </summary>
        /// <param name="DoctorOnlineSetLogInfo"></param>
        /// <returns></returns>
        Task<IList<DoctorOnlineSetLog>> DoctorOnlineSetLogList(DoctorOnlineSetLogIn DoctorOnlineSetLogInfo);
        /// <summary>
        /// 查询医生上下线设置log page
        /// </summary>
        /// <param name="DoctorOnlineSetLogInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<DoctorOnlineSetLog>> DoctorOnlineSetLogPage(DoctorOnlineSetLogIn DoctorOnlineSetLogInfo);
        /// <summary>
        /// 修改医生上下线设置log
        /// </summary>
        /// <param name="DoctorOnlineSetLogInfo"></param>
        /// <returns></returns>
        Task<DoctorOnlineSetLog> UpdateDoctorOnlineSetLog(DoctorOnlineSetLog DoctorOnlineSetLogInfo);
    }
}