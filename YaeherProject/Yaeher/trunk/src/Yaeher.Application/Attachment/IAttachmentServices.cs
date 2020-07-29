using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Yaeher
{
    /// <summary>
    /// 文件上传管理
    /// </summary>
    public interface IAttachmentServices : IApplicationService
    {
        /// <summary>
        /// 查询上传文件 List
        /// </summary>
        /// <param name="AttachmentsInfo"></param>
        /// <returns></returns>
        Task<IList<AttachmentService>> AttachmentList(AttachmentIn AttachmentsInfo);

        /// <summary>
        /// 查询咨询回答 List
        /// </summary>
        /// <param name="AttachmentsInfo"></param>
        /// <returns></returns>
        Task<IList<ReplyDetail>> ReplyDetailList(AttachmentIn AttachmentsInfo);
        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<AttachmentService> AttachmentServiceInfoByID(int Id);
        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        /// <param name="att"></param>
        /// <returns></returns>
        Task<AttachmentService> UpdateAttachmentService(AttachmentService att);
        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<AttachmentService> AttachmentServiceInfoByExpression(Expression<Func<AttachmentService, bool>> whereExpression);
        /// <summary>
        /// 查询文件上传管理 page
        /// </summary>
        /// <param name="AttachmentInfo"></param>
        /// <returns></returns>
        Task<PagedResultDto<AttachmentService>> AttachmentServicePage(AttachmentIn AttachmentInfo);
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="AttachmentsInfo"></param>
        /// <returns></returns>
        Task<AttachmentService> CreateAttachment(AttachmentService AttachmentsInfo);
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="AttachmentsInfo"></param>
        /// <returns></returns>
        Task InsertAttachment(AttachmentService AttachmentsInfo);
        /// <summary>
        /// 删除上传文件
        /// </summary>
        /// <param name="AttachmentsInfo"></param>
        /// <returns></returns>
        Task<AttachmentService> DeleteAttachment(AttachmentService AttachmentsInfo);
    }
}