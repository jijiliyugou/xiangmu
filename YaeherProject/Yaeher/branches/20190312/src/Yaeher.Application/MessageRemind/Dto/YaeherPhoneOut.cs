using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher.MessageRemind.Dto
{
    /// <summary>
    /// 电话记录
    /// </summary>
    public class YaeherPhoneOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherPhoneDto"></param>
        /// <param name="YaeherPhoneInfo"></param>
        public YaeherPhoneOut(PagedResultDto<YaeherPhone> YaeherPhoneDto, YaeherPhoneIn YaeherPhoneInfo)
        {
            Items = YaeherPhoneDto.Items;
            TotalCount = YaeherPhoneDto.TotalCount;
            TotalPage = YaeherPhoneDto.TotalCount / YaeherPhoneInfo.MaxResultCount;
            SkipCount = YaeherPhoneInfo.SkipCount;
            MaxResultCount = YaeherPhoneInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherPhone> Items { get; set; }
    }
}
