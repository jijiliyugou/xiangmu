using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yaeher.Doctor;
using Yaeher.YaeherDoctors.Dto;

namespace Yaeher.YaeherDoctors
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICollectConsultationService: IApplicationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<CollectConsultation> CollectConsultationByID(int Id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        Task<CollectConsultation> CreateCollectConsultation(CollectConsultation DoctorFileApplyInfo);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        Task<CollectConsultation> DeleteCollectConsultation(CollectConsultation DoctorFileApplyInfo);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DoctorFileApplyInfo"></param>
        /// <returns></returns>
        Task<CollectConsultation> UpdateCollectConsultation(CollectConsultation DoctorFileApplyInfo);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consultationin"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherConsultation>> CollectConsultationPage(CollectConsultationIn consultationin);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consultationin"></param>
        /// <returns></returns>
        Task<IList<CollectConsultation>> CollectConsultationList(CollectConsultationIn consultationin);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consultationin"></param>
        /// <returns></returns>
        Task<List<CollectConsultation>> CollectConsultationListAsync(CollectConsultationIn consultationin);
        
        /// <summary>
        /// 我的医生byExpression
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<CollectConsultation> CollectConsultationByExpression(Expression<Func<CollectConsultation, bool>> whereExpression);
    }
}