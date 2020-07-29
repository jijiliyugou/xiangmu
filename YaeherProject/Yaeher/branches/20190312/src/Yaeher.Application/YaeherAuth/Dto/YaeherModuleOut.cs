using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherAuth.Dto
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public class YaeherModuleOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherModuleDto"></param>
        /// <param name="YaeherModuleOutInfo"></param>
        public YaeherModuleOut(PagedResultDto<YaeherModule> YaeherModuleDto, YaeherModuleIn YaeherModuleOutInfo)
        {
            Items = YaeherModuleDto.Items;
            TotalCount = YaeherModuleDto.TotalCount;
            TotalPage = YaeherModuleDto.TotalCount / YaeherModuleOutInfo.MaxResultCount;
            SkipCount = YaeherModuleOutInfo.SkipCount;
            MaxResultCount = YaeherModuleOutInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherModule> Items { get; set; }

    }

}
