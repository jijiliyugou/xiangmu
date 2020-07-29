using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using System;
using System.Linq.Expressions;

namespace Yaeher
{
    /// <summary>
    /// 文件上传管理
    /// </summary>
    public class AttachmentServices : IAttachmentServices
    {
        private readonly IRepository<AttachmentService> _repository;
        /// <summary>
        /// 文件上传管理构造函数
        /// </summary>
        /// <param name="repository"></param>
        public AttachmentServices(IRepository<AttachmentService> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 查询上传文件 List
        /// </summary>
        /// <param name="AttachmentsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<AttachmentService>> AttachmentList(AttachmentIn AttachmentsInfo)
        {
            var Attachments = await _repository.GetAllListAsync(AttachmentsInfo.Expression);
            return Attachments.ToList();
        }
        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<AttachmentService> AttachmentServiceInfoByID(int Id)
        {
            var Attachments = await _repository.FirstOrDefaultAsync(t => t.Id == Id && !t.IsDelete);
            return Attachments;
        }
        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        /// <param name="att"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<AttachmentService> UpdateAttachmentService(AttachmentService att)
        {
            return await _repository.UpdateAsync(att);
        }
        
        /// <summary>
        /// 附件byExpression
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<AttachmentService> AttachmentServiceInfoByExpression(Expression<Func<AttachmentService, bool>> whereExpression)
        {
            var Attachments = await _repository.FirstOrDefaultAsync(whereExpression);
            return Attachments;
        }
        /// <summary>
        /// 查询文件上传管理 page
        /// </summary>
        /// <param name="AttachmentInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<PagedResultDto<AttachmentService>> AttachmentServicePage(AttachmentIn AttachmentInfo)
        {
            //初步过滤
            var query = _repository.GetAll().OrderByDescending(a => a.CreatedOn).Where(AttachmentInfo.Expression);
            //获取总数
            var tasksCount = query.Count();
            //获取总数
            var totalpage = tasksCount / AttachmentInfo.MaxResultCount;
            var AttachmentList = await query.PageBy(AttachmentInfo.SkipTotal, AttachmentInfo.MaxResultCount).ToListAsync();
            return new PagedResultDto<AttachmentService>(tasksCount, AttachmentList.MapTo<List<AttachmentService>>());
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="AttachmentsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<AttachmentService> CreateAttachment(AttachmentService AttachmentsInfo)
        {
            AttachmentsInfo.Id= await _repository.InsertAndGetIdAsync(AttachmentsInfo);
            return AttachmentsInfo;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="AttachmentsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task InsertAttachment(AttachmentService AttachmentsInfo)
        {
            await _repository.InsertAsync(AttachmentsInfo);
        }
        
        /// <summary>
        /// 删除上传文件
        /// </summary>
        /// <param name="AttachmentsInfo"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<AttachmentService> DeleteAttachment(AttachmentService AttachmentsInfo)
        {
            return await _repository.UpdateAsync(AttachmentsInfo);
        }

        /// <summary>
        /// 查询咨询回答明细 List
        /// </summary>
        /// <param name="AttachmentIn"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public async Task<IList<ReplyDetail>> ReplyDetailList(AttachmentIn AttachmentIn)
        {
            var query = _repository.GetAll().Where(t => !t.IsDelete);
            var querylist = from a in query
                            where  a.ConsultNumber == AttachmentIn.ConsultNumber
                            select new ReplyDetail
                            {
                                ReplyId = a.ServiceID,
                                ConsultNumber = a.ConsultNumber,
                                ReplyNumber=a.ServiceNumber,
                                FileName = a.Filename,
                                FileSize=a.FileSize.ToString(),
                                Mediatype=a.FileType,
                                ConsultID = a.ConsultID,
                                ServiceType = a.FileFrom,
                                CreatedOn = a.CreatedOn,
                                Message = a.FileAddress,
                                FileContentAddress = a.FileContentAddress,
                                FileTotalTime=a.FileTotalTime,
                                Id =a.Id
                            };
            return await querylist.ToListAsync();
        }
        
    }
}
