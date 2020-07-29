using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.Quality.Dto;

namespace Yaeher.Quality
{
    /// <summary>
    /// 质控委员会
    /// </summary>
    public interface IQualityCommitteeService : IApplicationService
    {
        /// <summary>
        /// 新建质控委员会
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        Task<QualityCommittee> CreateQualityCommittee(QualityCommittee QualityCommitteeInfo);
        /// <summary>
        /// 删除质控委员会
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        Task<QualityCommittee> DeleteQualityCommittee(QualityCommittee QualityCommitteeInfo);
        /// <summary>
        /// 查询质控委员会byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<QualityCommittee> QualityCommitteeByID(int Id);

        /// <summary>
        /// 查询质控委员会byId
        /// </summary>
        /// <param name="DoctorId"></param>
        /// <returns></returns>
        Task<QualityCommittee> QualityCommitteeByDoctorID(int DoctorId);
        
        /// <summary>
        /// 查询质控委员会 List
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        Task<IList<QualityCommittee>> QualityCommitteeList(QualityCommitteeIn QualityCommitteeInfo);
        /// <summary>
        /// 查询质控委员会 page
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<QualityCommittee>> QualityCommitteePage(QualityCommitteeIn QualityCommitteeInfo);
        /// <summary>
        /// 修改质控委员会
        /// </summary>
        /// <param name="QualityCommitteeInfo"></param>
        /// <returns></returns>
        Task<QualityCommittee> UpdateQualityCommittee(QualityCommittee QualityCommitteeInfo);
    }
}