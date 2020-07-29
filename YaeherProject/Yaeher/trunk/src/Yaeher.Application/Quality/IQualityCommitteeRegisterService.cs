using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.ClinicManage.Dto;
using Yaeher.DoctorQuality.Dto;
using Yaeher.Quality.Dto;

namespace Yaeher.Quality
{
    /// <summary>
    /// 质控委员注册
    /// </summary>
    public interface IQualityCommitteeRegisterService : IApplicationService
    {
        /// <summary>
        /// 新建质控委员注册
        /// </summary>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        /// <returns></returns>
        Task<QualityCommitteeRegister> CreateQualityCommitteeRegister(QualityCommitteeRegister QualityCommitteeRegisterInfo);
        /// <summary>
        /// 删除质控委员注册
        /// </summary>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        /// <returns></returns>
        Task<QualityCommitteeRegister> DeleteQualityCommitteeRegister(QualityCommitteeRegister QualityCommitteeRegisterInfo);
        /// <summary>
        /// 医生信息
        /// </summary>
        /// <param name="clinic"></param>
        ///  <param name="rel"></param>
        /// <returns></returns>
        Task<PagedResultDto<QualityManager>> QualityDoctorRegisterInformation(QualityCommitteeRegisterIn clinic, IList<DoctorRelation> rel);
        /// <summary>
        /// 医生信息
        /// </summary>
        /// <param name="clinic"></param>
        ///  <param name="rel"></param>
        /// <returns></returns>
        Task<PagedResultDto<QualityManager>> QualityDoctorInformation(QualityCommitteeIn clinic, IList<DoctorRelation> rel);
        /// <summary>
        /// 查询质控委员注册byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<QualityCommitteeRegister> QualityCommitteeRegisterByID(int Id);
        /// <summary>
        /// 注册医生信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<QualityManager> QualityCommitteeRegisterDoctorMsgByID(int Id);
        /// <summary>
        /// 查询质控委员注册 List
        /// </summary>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        /// <returns></returns>
        Task<IList<QualityCommitteeRegister>> QualityCommitteeRegisterList(QualityCommitteeRegisterIn QualityCommitteeRegisterInfo);
        /// <summary>
        /// 查询质控委员注册 page
        /// </summary>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<QualityCommitteeRegister>> QualityCommitteeRegisterPage(QualityCommitteeRegisterIn QualityCommitteeRegisterInfo);
        /// <summary>
        /// 修改质控委员注册
        /// </summary>
        /// <param name="QualityCommitteeRegisterInfo"></param>
        /// <returns></returns>
        Task<QualityCommitteeRegister> UpdateQualityCommitteeRegister(QualityCommitteeRegister QualityCommitteeRegisterInfo);
    }
}