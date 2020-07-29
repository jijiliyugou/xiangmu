using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 地区管理
    /// </summary>
    public class AreaManageOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="AreaManageDto"></param>
        /// <param name="AreaManageInfo"></param>
        public AreaManageOut(PagedResultDto<AreaManage> AreaManageDto, AreaManageIn AreaManageInfo)
        {
            Items = AreaManageDto.Items;
            TotalCount = AreaManageDto.TotalCount;
            TotalPage = AreaManageDto.TotalCount / AreaManageInfo.MaxResultCount;
            SkipCount = AreaManageInfo.SkipCount;
            MaxResultCount = AreaManageInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<AreaManage> Items { get; set; }


    }
}
