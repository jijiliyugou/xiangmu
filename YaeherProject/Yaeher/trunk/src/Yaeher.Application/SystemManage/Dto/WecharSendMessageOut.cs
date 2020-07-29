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
    public class WecharSendMessageOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="WecharSendMessageDto"></param>
        /// <param name="WecharSendMessageInfo"></param>
        public WecharSendMessageOut(PagedResultDto<WecharSendMessage> WecharSendMessageDto, WecharSendMessageIn WecharSendMessageInfo)
        {
            Items = WecharSendMessageDto.Items;
            TotalCount = WecharSendMessageDto.TotalCount;
            TotalPage = WecharSendMessageDto.TotalCount / WecharSendMessageInfo.MaxResultCount;
            SkipCount = WecharSendMessageInfo.SkipCount;
            MaxResultCount = WecharSendMessageInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<WecharSendMessage> Items { get; set; }
    }
}
