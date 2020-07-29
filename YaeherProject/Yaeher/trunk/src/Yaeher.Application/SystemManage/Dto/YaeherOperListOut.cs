using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.SystemManage.Dto
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    public class YaeherOperListOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherOperListDto"></param>
        /// <param name="YaeherOperListInfo"></param>
        public YaeherOperListOut(PagedResultDto<YaeherOperList> YaeherOperListDto, YaeherOperListIn YaeherOperListInfo)
        {
            Items = YaeherOperListDto.Items;
            TotalCount = YaeherOperListDto.TotalCount;
            TotalPage = YaeherOperListDto.TotalCount / YaeherOperListInfo.MaxResultCount;
            SkipCount = YaeherOperListInfo.SkipCount;
            MaxResultCount = YaeherOperListInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherOperList> Items { get; set; }


    }
}
