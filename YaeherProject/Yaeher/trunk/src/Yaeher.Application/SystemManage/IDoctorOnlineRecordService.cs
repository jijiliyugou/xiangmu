using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 医生上下线设置
    /// </summary>
    public interface IDoctorOnlineRecordService : IApplicationService
    {
        /// <summary>
        /// 新建医生上下线设置
        /// </summary>
        /// <param name="DoctorOnlineRecordInfo"></param>
        /// <returns></returns>
        Task<DoctorOnlineRecord> CreateDoctorOnlineRecord(DoctorOnlineRecord DoctorOnlineRecordInfo);
        /// <summary>
        /// 删除医生上下线设置
        /// </summary>
        /// <param name="DoctorOnlineRecordInfo"></param>
        /// <returns></returns>
        Task<DoctorOnlineRecord> DeleteDoctorOnlineRecord(DoctorOnlineRecord DoctorOnlineRecordInfo);
        /// <summary>
        /// 查询医生上下线设置byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DoctorOnlineRecord> DoctorOnlineRecordByID(int Id);
        /// <summary>
        /// 查询医生上下线设置byId
        /// </summary>
        /// <param name="DoctorId"></param>
        /// <returns></returns>
        Task<DoctorOnlineRecord> DoctorOnlineRecordByDoctorID(int DoctorId);
        /// <summary>
        /// 查询医生上下线设置byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DoctorOnlineRecord> DoctorOnlineRecordDoctorID(int Id);
        /// <summary>
        /// 查询医生上下线设置 List
        /// </summary>
        /// <param name="DoctorOnlineRecordInfo"></param>
        /// <returns></returns>
        Task<IList<DoctorOnlineRecord>> DoctorOnlineRecordList(DoctorOnlineRecordIn DoctorOnlineRecordInfo);
        /// <summary>
        /// 查询医生上下线设置 page
        /// </summary>
        /// <param name="DoctorOnlineRecordInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<DoctorOnlineRecord>> DoctorOnlineRecordPage(DoctorOnlineRecordIn DoctorOnlineRecordInfo);
        /// <summary>
        /// 修改医生上下线设置
        /// </summary>
        /// <param name="DoctorOnlineRecordInfo"></param>
        /// <returns></returns>
        Task<DoctorOnlineRecord> UpdateDoctorOnlineRecord(DoctorOnlineRecord DoctorOnlineRecordInfo);
    }
}