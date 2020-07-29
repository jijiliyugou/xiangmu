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
    public class MessageTemplateOut: PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="MessageTemplateDto"></param>
        /// <param name="MessageTemplateInfo"></param>
        public MessageTemplateOut(PagedResultDto<YaeherMessageTemplate> MessageTemplateDto, MessageTemplateIn MessageTemplateInfo)
        {
            Items = MessageTemplateDto.Items;
            TotalCount = MessageTemplateDto.TotalCount;
            TotalPage = MessageTemplateDto.TotalCount / MessageTemplateInfo.MaxResultCount;
            SkipCount = MessageTemplateInfo.SkipCount;
            MaxResultCount = MessageTemplateInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherMessageTemplate> Items { get; set; }
    }
}
