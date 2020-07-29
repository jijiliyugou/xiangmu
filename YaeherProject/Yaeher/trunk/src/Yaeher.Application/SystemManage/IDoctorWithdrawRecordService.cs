using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.SystemManage.Dto;

namespace Yaeher.SystemManage
{
    /// <summary>
    /// 医生提现记录
    /// </summary>
    public interface IDoctorWithdrawRecordService : IApplicationService
    {
        /// <summary>
        /// 新建医生提现记录
        /// </summary>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        /// <returns></returns>
        Task<DoctorWithdrawRecord> CreateDoctorWithdrawRecord(DoctorWithdrawRecord DoctorWithdrawRecordInfo);
        /// <summary>
        /// 删除医生提现记录
        /// </summary>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        /// <returns></returns>
        Task<DoctorWithdrawRecord> DeleteDoctorWithdrawRecord(DoctorWithdrawRecord DoctorWithdrawRecordInfo);
        /// <summary>
        /// 查询医生提现记录byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DoctorWithdrawRecord> DoctorWithdrawRecordByID(int Id);
        /// <summary>
        /// 查询医生提现记录 List
        /// </summary>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        /// <returns></returns>
        Task<IList<DoctorWithdrawRecord>> DoctorWithdrawRecordList(DoctorWithdrawRecordIn DoctorWithdrawRecordInfo);
        /// <summary>
        /// 查询医生提现记录 page
        /// </summary>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<DoctorWithdrawRecord>> DoctorWithdrawRecordPage(DoctorWithdrawRecordIn DoctorWithdrawRecordInfo);
        /// <summary>
        /// 修改医生提现记录
        /// </summary>
        /// <param name="DoctorWithdrawRecordInfo"></param>
        /// <returns></returns>
        Task<DoctorWithdrawRecord> UpdateDoctorWithdrawRecord(DoctorWithdrawRecord DoctorWithdrawRecordInfo);
    }
}