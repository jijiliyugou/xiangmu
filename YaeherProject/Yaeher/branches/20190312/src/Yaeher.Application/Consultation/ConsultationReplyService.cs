using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Yaeher.ClinicManage.Dto;
using Yaeher.Consultation.Dto;

namespace Yaeher.Consultation
{

    /// <summary>
    /// 咨询回答
    /// </summary>
    public class ConsultationReplyService : IConsultationReplyService
    {
        private readonly IRepository<ConsultationReply> _repository;
        /// <summary>
        ///  咨询回答
        /// </summary>
        /// <param name="repository"></param>
        public ConsultationReplyService(IRepository<ConsultationReply> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 查询咨询回答 List
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<ConsultationReply>> ConsultationReplyList(ConsultationReplyIn ConsultationReplyInfo)
        {
            var ConsultationReplys =  _repository.GetAll().Where(ConsultationReplyInfo.Expression).OrderByDescending(t=>t.CreatedOn);
            return await ConsultationReplys.ToListAsync();
        }
        /// <summary>
        /// 查询咨询回答明细 List
        /// </summary>
        /// <param name="ReplyDetailList"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<ReplyDetail>> ReplyDetailList(ConsultationReplyIn ReplyDetailList)
        {
            var query = _repository.GetAll();
            var querylist = from a in query
                            where a.ConsultNumber == ReplyDetailList.ConsultNumber && !a.IsDelete
                            select new ReplyDetail
                            {
                                ReplyId = a.Id,
                                ConsultNumber = a.ConsultNumber,
                                ReplyNumber =a.SequenceNo,
                                CreatedOn = a.CreatedOn,
                                CreatedBy = a.CreatedBy,
                                ReplyType=a.ReplyType,
                                Message = a.RepayIllnessDescription,
                                ConsultType= a.ConsultType,
                                AnswerType="Message",
                            };
            return await querylist.OrderBy(t=>t.CreatedOn).ToListAsync();
        }
        /// <summary>
        /// 查询咨询回答byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ConsultationReply> ConsultationReplyByID(int Id)
        {
            var ClinicDoctorReltions = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return ClinicDoctorReltions;
        }
        /// <summary>
        /// 查询咨询回答 page
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<ConsultationReply>> ConsultationReplyPage(ConsultationReplyIn ConsultationReplyInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(ConsultationReplyInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / ConsultationReplyInfo.MaxResultCount;
            var ClinicDoctorReltionList = await query.PageBy(ConsultationReplyInfo.SkipTotal* ConsultationReplyInfo.MaxResultCount, ConsultationReplyInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<ConsultationReply>(tasksCount, ClinicDoctorReltionList.MapTo<List<ConsultationReply>>());
        }
        /// <summary>
        /// 新建咨询回答
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ConsultationReply> CreateConsultationReply(ConsultationReply ConsultationReplyInfo)
        {
            ConsultationReplyInfo.Id= await _repository.InsertAndGetIdAsync(ConsultationReplyInfo);
            return ConsultationReplyInfo;
        }
        [RemoteService(false)]
        public async Task InsertConsultationReply(ConsultationReply ConsultationReplyInfo)
        {
             await _repository.InsertAndGetIdAsync(ConsultationReplyInfo);
        }
        
        /// <summary>
        /// 修改咨询回答
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ConsultationReply> UpdateConsultationReply(ConsultationReply ConsultationReplyInfo)
        {
            return await _repository.UpdateAsync(ConsultationReplyInfo);
        }

        /// <summary>
        /// 删除咨询回答
        /// </summary>
        /// <param name="ConsultationReplyInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<ConsultationReply> DeleteConsultationReply(ConsultationReply ConsultationReplyInfo)
        {
            return await _repository.UpdateAsync(ConsultationReplyInfo);
        }

    }
}
