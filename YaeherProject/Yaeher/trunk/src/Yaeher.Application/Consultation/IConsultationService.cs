using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.DoctorReport.Dto;
using Yaeher.NumericalStatement.Dto;

namespace Yaeher.Consultation
{
    /// <summary>
    /// 咨询管理
    /// </summary>
    public interface IConsultationService : IApplicationService
    {
        /// <summary>
        /// 新建咨询管理
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        Task<YaeherConsultation> CreateYaeherConsultation(YaeherConsultation YaeherConsultationInfo);
        /// <summary>
        /// 删除咨询管理
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        Task<YaeherConsultation> DeleteYaeherConsultation(YaeherConsultation YaeherConsultationInfo);
        /// <summary>
        /// 修改咨询管理
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        Task<YaeherConsultation> UpdateYaeherConsultation(YaeherConsultation YaeherConsultationInfo);
        /// <summary>
        /// 查询咨询管理byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<YaeherConsultation> YaeherConsultationByID(int Id);
        /// <summary>
        /// 查询咨询管理byConsulNumber
        /// </summary>
        /// <param name="ConsulNumber"></param>
        /// <returns></returns>
        Task<YaeherConsultation> YaeherConsultationByNumber(string ConsulNumber);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConsultNumber"></param>
        /// <returns></returns>
        Task<YaeherConsultation> ConsultationByNumber(string ConsultNumber);
        /// <summary>
        /// 查询咨询管理 List
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        Task<IList<YaeherConsultation>> YaeherConsultationList(ConsultationIn YaeherConsultationInfo);

        /// <summary>
        /// 查询咨询管理 List
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        Task<IList<DoctorNew>> DoctorNewList(DoctorNewIn YaeherConsultationInfo);
        /// <summary>
        /// 查询咨询管理 List
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        Task<IList<DoctorNew>> DoctorOldList(DoctorNewIn YaeherConsultationInfo);
        /// <summary>
        /// 查询咨询管理 page
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<YaeherConsultation>> YaeherConsultationPage(ConsultationIn YaeherConsultationInfo);
        /// <summary>
        /// 查询咨询管理 page
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<QualityConsultationPage>> QualityYaeherConsultationPage(ConsultationIn YaeherConsultationInfo);
        /// <summary>
        /// 查询咨询管理 page
        /// </summary>
        /// <param name="YaeherConsultationInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<QualityConsultationPage>> QualityRefundYaeherConsultationPage(ConsultationIn YaeherConsultationInfo);
        /// <summary>
        /// 管理 查询收入明细
        /// </summary>
        /// <param name="detailin"></param>
        /// <returns></returns>
        Task<List<AdminIncomeDetail>> IncomeConsultationDetail(IncomeDetailsIn detailin);
    }
}