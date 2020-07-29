using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class QuickReplyOut:PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="QuickReplyDto"></param>
        /// <param name="QuickReplyInfo"></param>
        public QuickReplyOut(PagedResultDto<QuickReply> QuickReplyDto, QuickReplyIn QuickReplyInfo)
        {
            Items = QuickReplyDto.Items;
            TotalCount = QuickReplyDto.TotalCount;
            TotalPage = QuickReplyDto.TotalCount / QuickReplyInfo.MaxResultCount;
            SkipCount = QuickReplyInfo.SkipCount;
            MaxResultCount = QuickReplyInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<QuickReply> Items { get; set; }
    }
}
