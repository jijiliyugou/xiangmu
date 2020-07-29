using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yaeher.ClinicManage.Dto;
using Yaeher.Consultation.Dto;

namespace Yaeher.Consultation
{
    /// <summary>
    /// 咨询回答
    /// </summary>
    public interface IConsultationReplyService : IApplicationService
    {
        /// <summary>
        /// 查询咨询回答byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ConsultationReply> ConsultationReplyByID(int Id);
        /// <summary>
        /// 查询咨询回答 List
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        Task<IList<ConsultationReply>> ConsultationReplyList(ConsultationReplyIn ConsultationReplyInfo);
        /// <summary>
        /// 查询咨询回答 List
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        Task<IList<ReplyDetail>> ReplyDetailList(ConsultationReplyIn ConsultationReplyInfo);
        /// <summary>
        /// 查询咨询回答 page
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<ConsultationReply>> ConsultationReplyPage(ConsultationReplyIn ConsultationReplyInfo);
        /// <summary>
        /// 新建咨询回答
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        Task<ConsultationReply> CreateConsultationReply(ConsultationReply ConsultationReplyInfo);

        Task InsertConsultationReply(ConsultationReply ConsultationReplyInfo);
        /// <summary>
        /// 修改咨询回答
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        Task<ConsultationReply> DeleteConsultationReply(ConsultationReply ConsultationReplyInfo);
        /// <summary>
        /// 删除咨询回答
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        Task<ConsultationReply> UpdateConsultationReply(ConsultationReply ConsultationReplyInfo);
    }
}