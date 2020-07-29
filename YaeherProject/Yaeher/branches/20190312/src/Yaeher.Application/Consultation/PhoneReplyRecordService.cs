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

namespace Yaeher.Consultation
{

    /// <summary>
    /// 电话回复记录
    /// </summary>
    public class PhoneReplyRecordService : IPhoneReplyRecordService
    {
        private readonly IRepository<PhoneReplyRecord> _repository;
        /// <summary>
        ///  电话回复记录构造函数
        /// </summary>
        /// <param name="repository"></param>
        public PhoneReplyRecordService(IRepository<PhoneReplyRecord> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 查询电话回复记录 List
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<PhoneReplyRecord>> PhoneReplyRecordList(PhoneReplyRecordIn PhoneReplyRecordInfo)
        {
            var PhoneReplyRecords = await _repository.GetAllListAsync(PhoneReplyRecordInfo.Expression);
            return PhoneReplyRecords.ToList();
        }

        /// <summary>
        /// 查询电话回复记录byId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PhoneReplyRecord> PhoneReplyRecordByID(int Id)
        {
            var PhoneReplyRecords = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return PhoneReplyRecords;
        }
        /// <summary>
        /// 查询电话回复记录 page
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<PhoneReplyRecord>> PhoneReplyRecordPage(PhoneReplyRecordIn PhoneReplyRecordInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(PhoneReplyRecordInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / PhoneReplyRecordInfo.MaxResultCount;
            var PhoneReplyRecordList = await query.PageBy(PhoneReplyRecordInfo.SkipTotal, PhoneReplyRecordInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<PhoneReplyRecord>(tasksCount, PhoneReplyRecordList.MapTo<List<PhoneReplyRecord>>());
        }
        /// <summary>
        /// 新建电话回复记录
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PhoneReplyRecord> CreatePhoneReplyRecord(PhoneReplyRecord PhoneReplyRecordInfo)
        {
            PhoneReplyRecordInfo.Id= await _repository.InsertAndGetIdAsync(PhoneReplyRecordInfo);
            return PhoneReplyRecordInfo;
        }
        /// <summary>
        /// 查询咨询回答明细 List
        /// </summary>
        /// <param name="ReplyDetailList"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<ReplyDetail>> ReplyDetailList(PhoneReplyRecordIn ReplyDetailList)
        {
            var query = _repository.GetAll();
            var querylist = from a in query
                            where !a.IsDelete&&a.ConsultNumber== ReplyDetailList.ConsultNumber
                            select new ReplyDetail
                            {
                                ReplyId=a.Id,
                                CreatedBy=a.CreatedBy,
                                CreatedOn= a.CreatedOn,
                                ReplyType = "answer",
                                ConsultType= "Phone",
                                Message = a.RecordAddress,
                                AnswerType= "Phone",
                            };
            return await querylist.ToListAsync();
        }
        
        /// <summary>
        /// 修改电话回复记录
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PhoneReplyRecord> UpdatePhoneReplyRecord(PhoneReplyRecord PhoneReplyRecordInfo)
        {
            return await _repository.UpdateAsync(PhoneReplyRecordInfo);
        }

        /// <summary>
        /// 删除电话回复记录
        /// </summary>
        /// <param name="PhoneReplyRecordInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PhoneReplyRecord> DeletePhoneReplyRecord(PhoneReplyRecord PhoneReplyRecordInfo)
        {
            return await _repository.UpdateAsync(PhoneReplyRecordInfo);
        }

    }
}
