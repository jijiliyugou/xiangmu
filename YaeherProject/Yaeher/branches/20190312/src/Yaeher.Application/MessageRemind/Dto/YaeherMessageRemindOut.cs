using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.MessageRemind.Dto
{
    /// <summary>
    /// 短信对接
    /// </summary>
    public class YaeherMessageRemindOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherMessageRemindDto"></param>
        /// <param name="YaeherMessageRemindInfo"></param>
        public YaeherMessageRemindOut(PagedResultDto<YaeherMessageRemind> YaeherMessageRemindDto, YaeherMessageRemindIn YaeherMessageRemindInfo)
        {
            Items = YaeherMessageRemindDto.Items;
            TotalCount = YaeherMessageRemindDto.TotalCount;
            TotalPage = YaeherMessageRemindDto.TotalCount / YaeherMessageRemindInfo.MaxResultCount;
            SkipCount = YaeherMessageRemindInfo.SkipCount;
            MaxResultCount = YaeherMessageRemindInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherMessageRemind> Items { get; set; }
    }
}
