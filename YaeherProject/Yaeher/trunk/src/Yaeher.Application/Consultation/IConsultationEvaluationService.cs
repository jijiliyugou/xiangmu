using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.Consultation.Dto;

namespace Yaeher.Consultation
{
    /// <summary>
    /// 咨询评分
    /// </summary>
    public interface IConsultationEvaluationService : IApplicationService
    {
        /// <summary>
        /// 咨询评分byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ConsultationEvaluation> ConsultationEvaluationByID(int Id);
        /// <summary>
        /// 咨询评分byNumber
        /// </summary>
        /// <param name="ConsultNumber"></param>
        /// <returns></returns>
        Task<ConsultationEvaluation> ConsultationEvaluationByNumber(string ConsultNumber);
        /// <summary>
        /// 咨询评分 List
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        Task<List<ConsultationEvaluation>> ConsultationEvaluationList(ConsultationEvaluationIn ConsultationEvaluationInfo);
        /// <summary>
        /// 咨询评分 page
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<ConsultationEvaluation>> ConsultationEvaluationPage(ConsultationEvaluationIn ConsultationEvaluationInfo);
        /// <summary>
        /// 新建咨询评分
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        Task<ConsultationEvaluation> CreateConsultationEvaluation(ConsultationEvaluation ConsultationEvaluationInfo);
        /// <summary>
        /// 删除咨询评分
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        Task<ConsultationEvaluation> DeleteConsultationEvaluation(ConsultationEvaluation ConsultationEvaluationInfo);
        /// <summary>
        /// 修改咨询评分
        /// </summary>
        /// <param name="ConsultationEvaluationInfo"></param>
        /// <returns></returns>
        Task<ConsultationEvaluation> UpdateConsultationEvaluation(ConsultationEvaluation ConsultationEvaluationInfo);
    }
}