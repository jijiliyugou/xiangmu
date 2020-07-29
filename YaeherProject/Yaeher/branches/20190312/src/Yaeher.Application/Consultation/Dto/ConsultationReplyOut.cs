using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using Yaeher.Consultation.Dto;

namespace Yaeher
{
    /// <summary>
    /// ConsultationReplyOut
    /// </summary>
    public class ConsultationReplyOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="ConsultationReplyDto"></param>
        /// <param name="ConsultationReplyInfo"></param>
        public ConsultationReplyOut(PagedResultDto<ConsultationReply> ConsultationReplyDto, ConsultationReplyIn ConsultationReplyInfo)
        {
            Items = ConsultationReplyDto.Items;
            TotalCount = ConsultationReplyDto.TotalCount;
            TotalPage = ConsultationReplyDto.TotalCount / ConsultationReplyInfo.MaxResultCount;
            SkipCount = ConsultationReplyInfo.SkipCount;
            MaxResultCount = ConsultationReplyInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<ConsultationReply> Items { get; set; }
    }
    /// <summary>
    /// 回复类型list
    /// </summary>
    public class ConsultationReplyCodeList
    {
        /// <summary>
        /// 状态
        /// </summary>
        public List<CodeList> ReplyState { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public List<CodeList> ConsultState { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public List<CodeList> ReplyType { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="replyState"></param>
        /// <param name="replyType"></param>
        /// <param name="consultState"></param>
        /// <param name="MaxReplyLength"></param>
        /// <param name="MaxConsultationLength"></param>
        public ConsultationReplyCodeList(List<CodeList> replyState, List<CodeList> replyType, List<CodeList> consultState, int MaxReplyLength,int MaxConsultationLength)
        {
            ReplyState=replyState;
            ReplyType = replyType;
            ConsultState = consultState;
            maxReplyLength = MaxReplyLength;
            maxConsultationLength = MaxConsultationLength;
        }
        /// <summary>
        /// 最大追问字数
        /// </summary>
        public int maxReplyLength { get; set; }

        /// <summary>
        /// 最大咨询字数
        /// </summary>
        public int maxConsultationLength { get; set; }
    }
}
