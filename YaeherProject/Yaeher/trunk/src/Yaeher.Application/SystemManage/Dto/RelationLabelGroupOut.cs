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
    public class RelationLabelGroupOut: PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="RelationLabelGroupDto"></param>
        /// <param name="RelationLabelGroupInfo"></param>
        public RelationLabelGroupOut(PagedResultDto<RelationLabelGroup> RelationLabelGroupDto, RelationLabelGroupIn RelationLabelGroupInfo)
        {
            Items = RelationLabelGroupDto.Items;
            TotalCount = RelationLabelGroupDto.TotalCount;
            TotalPage = RelationLabelGroupDto.TotalCount / RelationLabelGroupInfo.MaxResultCount;
            SkipCount = RelationLabelGroupInfo.SkipCount;
            MaxResultCount = RelationLabelGroupInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<RelationLabelGroup> Items { get; set; }
    }
}
