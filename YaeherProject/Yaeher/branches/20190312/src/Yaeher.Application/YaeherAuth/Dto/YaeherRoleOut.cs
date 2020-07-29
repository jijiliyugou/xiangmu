using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.YaeherAuth.Dto
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class YaeherRoleOut : PagerViewModel
    {
        /// <summary>
        /// 输出模型
        /// </summary>
        /// <param name="YaeherRoleDto"></param>
        /// <param name="YaeherRoleInfo"></param>
        public YaeherRoleOut(PagedResultDto<YaeherRole> YaeherRoleDto, YaeherRoleIn YaeherRoleInfo)
        {
            Items = YaeherRoleDto.Items;
            TotalCount = YaeherRoleDto.TotalCount;
            TotalPage = YaeherRoleDto.TotalCount / YaeherRoleInfo.MaxResultCount;
            SkipCount = YaeherRoleInfo.SkipCount;
            MaxResultCount = YaeherRoleInfo.MaxResultCount;
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public IReadOnlyList<YaeherRole> Items { get; set; }


    }

}
