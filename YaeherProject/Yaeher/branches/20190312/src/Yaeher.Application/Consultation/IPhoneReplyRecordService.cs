using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Yaeher.Consultation
{
    /// <summary>
    /// 电话回复记录
    /// </summary>
    public interface IPhoneReplyRecordService : IApplicationService
    {
        /// <summary>
        /// 新建电话回复记录
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        Task<PhoneReplyRecord> CreatePhoneReplyRecord(PhoneReplyRecord PhoneReplyRecordInfo);
        /// <summary>
        /// 删除电话回复记录
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        Task<PhoneReplyRecord> DeletePhoneReplyRecord(PhoneReplyRecord PhoneReplyRecordInfo);
        /// <summary>
        /// 查询电话回复记录byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<PhoneReplyRecord> PhoneReplyRecordByID(int Id);
        /// <summary>
        /// 查询电话回复记录 List
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        Task<IList<PhoneReplyRecord>> PhoneReplyRecordList(PhoneReplyRecordIn PhoneReplyRecordInfo);
        /// <summary>
        /// 查询咨询回答 List
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        Task<IList<ReplyDetail>> ReplyDetailList(PhoneReplyRecordIn ConsultationReplyInfo);
       
        /// <summary>
        /// 查询电话回复记录 page
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<PhoneReplyRecord>> PhoneReplyRecordPage(PhoneReplyRecordIn PhoneReplyRecordInfo);
        /// <summary>
        /// 修改电话回复记录
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        Task<PhoneReplyRecord> UpdatePhoneReplyRecord(PhoneReplyRecord PhoneReplyRecordInfo);
    }
}