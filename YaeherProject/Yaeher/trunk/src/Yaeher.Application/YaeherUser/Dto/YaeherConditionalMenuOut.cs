using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.SystemConfig;

namespace Yaeher
{
    /// <summary>
    /// 微信个性化菜单
    /// </summary>
    public class YaeherConditionalMenuOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherConditionalMenuDto"></param>
        /// <param name="YaeherConditionalMenuInfo"></param>
        public YaeherConditionalMenuOut(PagedResultDto<YaeherConditionalMenu> YaeherConditionalMenuDto, YaeherConditionalMenuIn YaeherConditionalMenuInfo)
        {
            Items = YaeherConditionalMenuDto.Items;
            TotalCount = YaeherConditionalMenuDto.TotalCount;
            TotalPage = YaeherConditionalMenuDto.TotalCount / YaeherConditionalMenuInfo.MaxResultCount;
            SkipCount = YaeherConditionalMenuInfo.SkipCount;
            MaxResultCount = YaeherConditionalMenuInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherConditionalMenu> Items { get; set; }

    }
}
