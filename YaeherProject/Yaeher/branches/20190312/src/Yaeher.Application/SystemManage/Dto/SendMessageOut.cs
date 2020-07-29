using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.SystemManage.Dto
{
    public class SendMessageOut: PagerViewModel
    {/// <summary>
     /// 输出模型
     /// </summary>
     /// <param name="SendMessageTemplateDto"></param>
     /// <param name="SendMessageInfo"></param>
        public SendMessageOut(PagedResultDto<SendMessageTemplate> SendMessageTemplateDto, SendMessageIn SendMessageInfo)
        {
            Items = SendMessageTemplateDto.Items;
            TotalCount = SendMessageTemplateDto.TotalCount;
            TotalPage = SendMessageTemplateDto.TotalCount / SendMessageInfo.MaxResultCount;
            SkipCount = SendMessageInfo.SkipCount;
            MaxResultCount = SendMessageInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<SendMessageTemplate> Items { get; set; }
    }
}
