using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.YaeherDoctors
{
    /// <summary>
    /// 医生上传附件
    /// </summary>
    public interface IDoctorFileApplyService : IApplicationService
    {
        /// <summary>
        /// 新建医生上传附件
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        Task<DoctorFileApply> CreateDoctorFileApply(DoctorFileApply DoctorFileApplyInfo);
        /// <summary>
        /// 删除医生上传附件
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        Task<DoctorFileApply> DeleteDoctorFileApply(DoctorFileApply DoctorFileApplyInfo);
        /// <summary>
        /// 查询医生上传附件byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DoctorFileApply> DoctorFileApplyByID(int Id);
        /// <summary>
        /// 查询医生上传附件 List
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        Task<List<DoctorFileApply>> DoctorFileApplyList(DoctorFileApplyIn DoctorFileApplyInfo);
        /// <summary>
        /// 查询医生上传附件 page
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<DoctorFileApply>> DoctorFileApplyPage(DoctorFileApplyIn DoctorFileApplyInfo);
        /// <summary>
        /// 修改医生上传附件
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        Task<DoctorFileApply> UpdateDoctorFileApply(DoctorFileApply DoctorFileApplyInfo);
    }
}